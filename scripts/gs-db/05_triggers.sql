CREATE OR REPLACE TRIGGER trg_clima_pos_analise
AFTER INSERT ON TB_ANALISE
FOR EACH ROW
BEGIN
  INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade)
  VALUES (:NEW.id, 25, 100, 65);
END;
/

CREATE OR REPLACE TRIGGER trg_valida_score
BEFORE INSERT OR UPDATE ON TB_RECOMENDACAO
FOR EACH ROW
BEGIN
  IF :NEW.score > 100 THEN
    :NEW.score := 100;
  ELSIF :NEW.score < 0 THEN
    :NEW.score := 0;
  END IF;
END;
/

CREATE OR REPLACE TRIGGER trg_alerta_score_baixo
AFTER INSERT ON TB_RECOMENDACAO
FOR EACH ROW
DECLARE
  v_id_usuario TB_ANALISE.id_usuario%TYPE;
  v_id_cultura TB_ANALISE.id_cultura%TYPE;
BEGIN
  IF :NEW.score < 70 THEN
    SELECT id_usuario, id_cultura INTO v_id_usuario, v_id_cultura
    FROM TB_ANALISE WHERE id = :NEW.id_analise;

    INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem)
    VALUES (v_id_usuario, v_id_cultura, 'APTIDAO', 'Score baixo: cultura pouco adequada para a regiao.');
  END IF;
END;
/