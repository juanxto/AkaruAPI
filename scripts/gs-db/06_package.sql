CREATE OR REPLACE PACKAGE pkg_relatorios AS
  PROCEDURE relatorio_analises_por_cultura;
  PROCEDURE relatorio_alertas_por_usuario;
  FUNCTION fn_total_analises RETURN NUMBER;
  FUNCTION fn_cultura_mais_analisada RETURN VARCHAR2;
END pkg_relatorios;
/

CREATE OR REPLACE PACKAGE BODY pkg_relatorios AS

  PROCEDURE relatorio_analises_por_cultura AS
    CURSOR c_cult IS
      SELECT c.nome AS cultura, COUNT(a.id) AS total
      FROM TB_CULTURA c
      LEFT JOIN TB_ANALISE a ON a.id_cultura = c.id
      GROUP BY c.nome
      ORDER BY total DESC;
  BEGIN
    DBMS_OUTPUT.PUT_LINE('=== Analises por cultura ===');
    FOR reg IN c_cult LOOP
      DBMS_OUTPUT.PUT_LINE(reg.cultura || ': ' || reg.total || ' analise(s)');
    END LOOP;
  END;

  PROCEDURE relatorio_alertas_por_usuario AS
    CURSOR c_user IS
      SELECT u.nome AS usuario, COUNT(al.id) AS total
      FROM TB_USUARIO u
      LEFT JOIN TB_ALERTA al ON al.id_usuario = u.id
      GROUP BY u.nome
      ORDER BY total DESC;
  BEGIN
    DBMS_OUTPUT.PUT_LINE('=== Alertas por usuario ===');
    FOR reg IN c_user LOOP
      DBMS_OUTPUT.PUT_LINE(reg.usuario || ': ' || reg.total || ' alerta(s)');
    END LOOP;
  END;

  FUNCTION fn_total_analises RETURN NUMBER AS
    v_total NUMBER;
  BEGIN
    SELECT COUNT(*) INTO v_total FROM TB_ANALISE;
    RETURN v_total;
  END;

  FUNCTION fn_cultura_mais_analisada RETURN VARCHAR2 AS
    v_nome VARCHAR2(100);
  BEGIN
    SELECT cultura INTO v_nome FROM (
      SELECT c.nome AS cultura, COUNT(a.id) AS total
      FROM TB_CULTURA c
      JOIN TB_ANALISE a ON a.id_cultura = c.id
      GROUP BY c.nome
      ORDER BY total DESC
    ) WHERE ROWNUM = 1;
    RETURN v_nome;
  END;

END pkg_relatorios;
/