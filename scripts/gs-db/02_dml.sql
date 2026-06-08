-- USUÁRIOS (10)
INSERT INTO TB_USUARIO (nome, email, senha, latitude, longitude) VALUES ('João Silva', 'joao@email.com', 'senha123', -5.79, -35.21);
INSERT INTO TB_USUARIO (nome, email, senha, latitude, longitude) VALUES ('Maria Souza', 'maria@email.com', 'senha123', -8.05, -34.88);
INSERT INTO TB_USUARIO (nome, email, senha, latitude, longitude) VALUES ('Pedro Santos', 'pedro@email.com', 'senha123', -15.78, -47.92);
INSERT INTO TB_USUARIO (nome, email, senha, latitude, longitude) VALUES ('Ana Lima', 'ana@email.com', 'senha123', -12.97, -38.50);
INSERT INTO TB_USUARIO (nome, email, senha, latitude, longitude) VALUES ('Carlos Oliveira', 'carlos@email.com', 'senha123', -16.68, -49.25);
INSERT INTO TB_USUARIO (nome, email, senha, latitude, longitude) VALUES ('Julia Costa', 'julia@email.com', 'senha123', -3.73, -38.52);
INSERT INTO TB_USUARIO (nome, email, senha, latitude, longitude) VALUES ('Rafael Alves', 'rafael@email.com', 'senha123', -7.12, -34.88);
INSERT INTO TB_USUARIO (nome, email, senha, latitude, longitude) VALUES ('Beatriz Rocha', 'beatriz@email.com', 'senha123', -10.91, -37.07);
INSERT INTO TB_USUARIO (nome, email, senha, latitude, longitude) VALUES ('Lucas Pereira', 'lucasp@email.com', 'senha123', -20.44, -54.64);
INSERT INTO TB_USUARIO (nome, email, senha, latitude, longitude) VALUES ('Fernanda Dias', 'fernanda@email.com', 'senha123', -1.45, -48.50);

-- CULTURAS (12)
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Milho', 120, 18, 32, 600, 'Argiloso');
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Feijão', 90, 15, 30, 400, 'Arenoso');
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Soja', 130, 20, 35, 700, 'Argiloso');
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Arroz', 140, 22, 36, 1200, 'Argiloso');
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Tomate', 110, 18, 30, 500, 'Arenoso');
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Cana-de-açúcar', 365, 20, 38, 1500, 'Argiloso');
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Mandioca', 300, 20, 35, 800, 'Arenoso');
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Algodão', 180, 22, 35, 700, 'Argiloso');
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Café', 270, 18, 28, 1400, 'Argiloso');
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Alface', 60, 10, 24, 300, 'Arenoso');
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Batata', 100, 15, 25, 500, 'Arenoso');
INSERT INTO TB_CULTURA (nome, ciclo_dias, temp_min, temp_max, chuva_ideal, tipo_solo) VALUES ('Trigo', 120, 10, 24, 450, 'Argiloso');

-- ANÁLISES (15)
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (1, 1, 'Plantio de verão', -5.79, -35.21);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (1, 2, 'Rotação de cultura', -5.79, -35.21);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (2, 3, 'Área irrigada', -8.05, -34.88);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (3, 4, 'Solo úmido', -15.78, -47.92);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (4, 5, 'Estufa', -12.97, -38.50);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (5, 6, 'Plantio comercial', -16.68, -49.25);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (6, 7, 'Sequeiro', -3.73, -38.52);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (7, 8, 'Grande área', -7.12, -34.88);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (8, 9, 'Encosta', -10.91, -37.07);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (9, 3, 'Plantio direto', -20.44, -54.64);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (10, 7, 'Horta familiar', -1.45, -48.50);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (2, 1, 'Teste de safra', -8.05, -34.88);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (3, 10, 'Hidroponia', -15.78, -47.92);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (4, 11, 'Inverno', -12.97, -38.50);
INSERT INTO TB_ANALISE (id_usuario, id_cultura, detalhes, latitude, longitude) VALUES (5, 12, 'Clima ameno', -16.68, -49.25);

-- RECOMENDAÇÕES (15)
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (1, 'Plantio recomendado no início das chuvas.', 85);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (2, 'Cultura adequada para rotação no período.', 78);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (3, 'Boa aptidão com irrigação complementar.', 90);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (4, 'Necessário monitorar excesso de chuva.', 72);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (5, 'Ambiente controlado favorece a cultura.', 88);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (6, 'Excelente aptidão para a região.', 95);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (7, 'Risco moderado por baixa chuva.', 60);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (8, 'Cultura bem adaptada ao clima.', 82);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (9, 'Atenção ao relevo da área.', 68);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (10, 'Alta produtividade esperada.', 91);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (11, 'Adequada para cultivo familiar.', 80);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (12, 'Período favorável ao plantio.', 84);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (13, 'Recomendado sistema hidropônico.', 76);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (14, 'Boa janela de plantio no inverno.', 79);
INSERT INTO TB_RECOMENDACAO (id_analise, texto, score) VALUES (15, 'Clima ameno favorece o desenvolvimento.', 87);

-- HISTÓRICO CLIMÁTICO (15)
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (1, 28.5, 120, 70);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (2, 27.0, 90, 65);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (3, 30.2, 150, 75);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (4, 26.8, 200, 80);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (5, 24.5, 60, 60);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (6, 31.0, 180, 72);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (7, 29.5, 40, 55);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (8, 30.8, 95, 68);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (9, 25.3, 110, 73);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (10, 28.9, 160, 77);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (11, 27.7, 130, 71);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (12, 29.1, 100, 66);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (13, 22.4, 50, 58);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (14, 20.0, 70, 62);
INSERT INTO TB_CLIMA_HIST (id_analise, temp_media, chuva_mm, umidade) VALUES (15, 23.6, 80, 64);

-- ALERTAS (15)
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (1, 1, 'CHUVA', 'Excesso de chuva previsto para a semana.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (1, 2, 'TEMPERATURA', 'Temperatura acima do ideal.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (2, 3, 'SECA', 'Período de estiagem identificado.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (3, 4, 'CHUVA', 'Risco de alagamento na área.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (4, 5, 'TEMPERATURA', 'Noites frias podem afetar a cultura.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (5, 6, 'UMIDADE', 'Umidade elevada, atenção a fungos.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (6, 7, 'SECA', 'Baixa precipitação no período.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (7, 8, 'VENTO', 'Ventos fortes previstos.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (8, 9, 'TEMPERATURA', 'Calor intenso nos próximos dias.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (9, 3, 'CHUVA', 'Chuvas irregulares na região.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (10, 7, 'UMIDADE', 'Umidade baixa, recomenda-se irrigação.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (2, 1, 'TEMPERATURA', 'Variação térmica acentuada.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (3, 10, 'GEADA', 'Risco de geada na madrugada.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (4, 11, 'SECA', 'Solo com baixa retenção de água.');
INSERT INTO TB_ALERTA (id_usuario, id_cultura, tipo, mensagem) VALUES (5, 12, 'CHUVA', 'Previsão de chuva acima da média.');

COMMIT;


DECLARE
  CURSOR c_culturas IS
    SELECT id, nome FROM TB_CULTURA WHERE ROWNUM <= 5;
  v_id       TB_CULTURA.id%TYPE;
  v_nome     TB_CULTURA.nome%TYPE;
  v_contador NUMBER := 1;
BEGIN
  WHILE v_contador <= 3 LOOP
    DBMS_OUTPUT.PUT_LINE('Contador (WHILE): ' || v_contador);
    v_contador := v_contador + 1;
  END LOOP;

  OPEN c_culturas;
  LOOP
    FETCH c_culturas INTO v_id, v_nome;
    EXIT WHEN c_culturas%NOTFOUND;
    DBMS_OUTPUT.PUT_LINE('Cultura ' || v_id || ': ' || v_nome);
  END LOOP;
  CLOSE c_culturas;
END;
/