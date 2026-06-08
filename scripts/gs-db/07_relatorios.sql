-- Relatorio 1: analises com usuario e cultura
SELECT a.id, u.nome AS usuario, c.nome AS cultura, a.dt_analise
FROM TB_ANALISE a
JOIN TB_USUARIO u ON u.id = a.id_usuario
JOIN TB_CULTURA c ON c.id = a.id_cultura
ORDER BY a.id;

-- Relatorio 2: recomendacoes com cultura e score
SELECT r.id, c.nome AS cultura, r.score, r.texto
FROM TB_RECOMENDACAO r
JOIN TB_ANALISE a ON a.id = r.id_analise
JOIN TB_CULTURA c ON c.id = a.id_cultura
ORDER BY r.score DESC;

-- Relatorio 3: alertas por usuario e cultura
SELECT al.id, u.nome AS usuario, c.nome AS cultura, al.tipo, al.mensagem
FROM TB_ALERTA al
JOIN TB_USUARIO u ON u.id = al.id_usuario
JOIN TB_CULTURA c ON c.id = al.id_cultura
ORDER BY u.nome;

-- Relatorio 4: dados climaticos por analise
SELECT ch.id, u.nome AS usuario, c.nome AS cultura, ch.temp_media, ch.chuva_mm, ch.umidade
FROM TB_CLIMA_HIST ch
JOIN TB_ANALISE a ON a.id = ch.id_analise
JOIN TB_USUARIO u ON u.id = a.id_usuario
JOIN TB_CULTURA c ON c.id = a.id_cultura
ORDER BY ch.id;

-- Relatorio 5: total de analises por usuario
SELECT u.nome AS usuario, COUNT(a.id) AS total_analises
FROM TB_USUARIO u
LEFT JOIN TB_ANALISE a ON a.id_usuario = u.id
GROUP BY u.nome
ORDER BY total_analises DESC;