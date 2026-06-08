
DECLARE
  v_total_usuarios NUMBER;
  v_total_culturas NUMBER;
BEGIN
  SELECT COUNT(*) INTO v_total_usuarios FROM TB_USUARIO;
  SELECT COUNT(*) INTO v_total_culturas FROM TB_CULTURA;
  DBMS_OUTPUT.PUT_LINE('Usuarios cadastrados: ' || v_total_usuarios);
  DBMS_OUTPUT.PUT_LINE('Culturas cadastradas: ' || v_total_culturas);
END;
/


DECLARE
  v_score       TB_RECOMENDACAO.score%TYPE;
  v_classificacao VARCHAR2(20);
BEGIN
  SELECT score INTO v_score FROM TB_RECOMENDACAO WHERE id = 6;
  IF v_score >= 85 THEN
    v_classificacao := 'OTIMA';
  ELSIF v_score >= 70 THEN
    v_classificacao := 'BOA';
  ELSE
    v_classificacao := 'BAIXA';
  END IF;
  DBMS_OUTPUT.PUT_LINE('Score: ' || v_score || ' - Aptidao: ' || v_classificacao);
END;
/


DECLARE
  v_media NUMBER;
BEGIN
  SELECT AVG(score) INTO v_media FROM TB_RECOMENDACAO;
  DBMS_OUTPUT.PUT_LINE('Media geral de score: ' || ROUND(v_media, 2));
  IF v_media >= 80 THEN
    DBMS_OUTPUT.PUT_LINE('Resultado: culturas bem adaptadas no geral.');
  ELSE
    DBMS_OUTPUT.PUT_LINE('Resultado: revisar aptidao das culturas.');
  END IF;
END;
/