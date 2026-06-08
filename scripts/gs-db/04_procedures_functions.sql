CREATE OR REPLACE PROCEDURE registrar_analise (
  p_id_usuario IN NUMBER,
  p_id_cultura IN NUMBER,
  p_detalhes   IN VARCHAR2,
  p_latitude   IN NUMBER,
  p_longitude  IN NUMBER
) AS
BEGIN
  INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude)
  VALUES (p_id_usuario, p_id_cultura, p_detalhes, p_latitude, p_longitude);
  COMMIT;
  DBMS_OUTPUT.PUT_LINE('Analise registrada com sucesso.');
END;
/

CREATE OR REPLACE PROCEDURE gerar_alerta (
  p_id_usuario IN NUMBER,
  p_id_cultura IN NUMBER,
  p_tipo       IN VARCHAR2,
  p_mensagem   IN VARCHAR2
) AS
BEGIN
  INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem)
  VALUES (p_id_usuario, p_id_cultura, p_tipo, p_mensagem);
  COMMIT;
  DBMS_OUTPUT.PUT_LINE('Alerta gerado para o usuario ' || p_id_usuario);
END;
/

CREATE OR REPLACE PROCEDURE listar_analises_usuario (
  p_id_usuario IN NUMBER
) AS
  CURSOR c_analises IS
    SELECT a.id, c.nome AS cultura, a.dt_analise
    FROM TB_ANALISE a
    JOIN TB_CULTURA c ON c.id = a.id_cultura
    WHERE a.id_usuario = p_id_usuario;
BEGIN
  FOR reg IN c_analises LOOP
    DBMS_OUTPUT.PUT_LINE('Analise ' || reg.id || ' - ' || reg.cultura || ' em ' || reg.dt_analise);
  END LOOP;
END;
/

CREATE OR REPLACE FUNCTION fn_score_climatico (
  p_id_cultura IN NUMBER,
  p_temp_media IN NUMBER
) RETURN NUMBER AS
  v_temp_min TB_CULTURA.temp_min%TYPE;
  v_temp_max TB_CULTURA.temp_max%TYPE;
  v_score    NUMBER;
BEGIN
  SELECT temp_min, temp_max INTO v_temp_min, v_temp_max
  FROM TB_CULTURA WHERE id = p_id_cultura;

  IF p_temp_media BETWEEN v_temp_min AND v_temp_max THEN
    v_score := 100;
  ELSIF p_temp_media < v_temp_min THEN
    v_score := 100 - ((v_temp_min - p_temp_media) * 10);
  ELSE
    v_score := 100 - ((p_temp_media - v_temp_max) * 10);
  END IF;

  IF v_score < 0 THEN
    v_score := 0;
  END IF;

  RETURN v_score;
END;
/

CREATE OR REPLACE FUNCTION fn_total_alertas_usuario (
  p_id_usuario IN NUMBER
) RETURN NUMBER AS
  v_total NUMBER;
BEGIN
  SELECT COUNT(*) INTO v_total FROM TB_ALERTA WHERE id_usuario = p_id_usuario;
  RETURN v_total;
END;
/

CREATE OR REPLACE FUNCTION fn_media_score_cultura (
  p_id_cultura IN NUMBER
) RETURN NUMBER AS
  v_media NUMBER;
BEGIN
  SELECT AVG(r.score) INTO v_media
  FROM TB_RECOMENDACAO r
  JOIN TB_ANALISE a ON a.id = r.id_analise
  WHERE a.id_cultura = p_id_cultura;
  RETURN NVL(ROUND(v_media, 2), 0);
END;
/