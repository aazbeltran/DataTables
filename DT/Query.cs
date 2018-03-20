#region Copyright

/*The MIT License (MIT)

Copyright (c) 2018 Alonso Alejandro Zúñiga Beltrán

Se concede permiso, de forma gratuita, a cualquier persona que obtenga una copia de este software y
de los archivos de documentación asociados (el "Software"), para utilizar el Software sin
restricción, incluyendo sin limitación los derechos a usar, copiar, modificar, fusionar, publicar,
distribuir, sublicenciar, y/o vender copias del Software, y a permitir a las personas a las que se
les proporcione el Software a hacer lo mismo, sujeto a las siguientes condiciones:

El aviso de copyright anterior y este aviso de permiso se incluirán en todas las copias o partes
sustanciales del Software.

EL SOFTWARE SE PROPORCIONA "TAL CUAL", SIN GARANTÍA DE NINGÚN TIPO, EXPRESA O IMPLÍCITA,
INCLUYENDO PERO NO LIMITADO A GARANTÍAS DE COMERCIALIZACIÓN, IDONEIDAD PARA UN PROPÓSITO
PARTICULAR Y NO INFRACCIÓN. EN NINGÚN CASO LOS AUTORES O TITULARES DEL COPYRIGHT SERÁN
RESPONSABLES DE NINGUNA RECLAMACIÓN, DAÑOS U OTRAS RESPONSABILIDADES, YA SEA EN UNA ACCIÓN DE
CONTRATO, AGRAVIO O CUALQUIER OTRO MOTIVO, QUE SURJA DE O EN CONEXIÓN CON EL SOFTWARE O EL USO U
OTRO TIPO DE ACCIONES EN EL SOFTWARE.*/

#endregion Copyright

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DT
{
    /// <summary>
    /// Representa una consulta.
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Lista de columnas a seleccionar.
        /// </summary>
        public List<string> select { get; set; }

        /// <summary>
        /// Tabla principal.
        /// </summary>
        public Table from { get; set; }

        /// <summary>
        /// Lista de relaciones.
        /// </summary>
        public List<Join> joins { get; set; }

        /// <summary>
        /// Lista de condicionales.
        /// </summary>
        public List<Where> wheres { get; set; }

        /// <summary>
        /// Cláusula de agrupación.
        /// </summary>
        public string groupBy { get; set; }

        /// <summary>
        /// Cláusula de ordenamiento.
        /// </summary>
        public string orderBy { get; set; }

        /// <summary>
        /// Crea una nueva instancia de Query
        /// con propiedades como parámetros.
        /// </summary>
        /// <param name="select">Lista de columnas a seleccionar.</param>
        /// <param name="from">Tabla principal.</param>
        /// <param name="joins">Lista de relaciones.</param>
        /// <param name="wheres">Lista de condicionales.</param>
        /// <param name="groupBy">Cláusula de agrupación.</param>
        /// <param name="orderBy">Cláusula de ordenamiento.</param>
        public Query(List<string> select, Table from, List<Join> joins = null, List<Where> wheres = null, string groupBy = null, string orderBy = null)
        {
            this.select = select;
            this.from = from;
            this.joins = joins;
            this.wheres = wheres;
            this.groupBy = groupBy;
            this.orderBy = orderBy;
        }

        /// <summary>
        /// Crea una nueva instancia de Query.
        /// </summary>
        public Query()
        {
        }

        /// <summary>
        /// Crea una nueva instancia de Query.
        /// </summary>
        /// <returns>Retorna la consulta formateada a T-SQL.</returns>
        public string GenerarConsulta()
        {
            int i = 0;
            // Se inicializa la consulta
            StringBuilder q = new StringBuilder("SELECT * FROM (SELECT ");

            // Se crea el select
            foreach (var s in this.select)
            {
                q.AppendFormat("{0}{1}", i == 0 ? "" : ",", s);
                i++;
            }

            // Se agrega el from
            q.AppendFormat(" FROM {0} {1}", this.from.nombre, this.from.alias != null ? this.from.alias : "");

            // Se agregan las relaciones en caso de existir
            if (this.joins != null && this.joins.Count > 0)
            {
                foreach (var j in this.joins)
                {
                    // Según el tipo de relación se agrega en un formato en específico
                    if (j.tipo.Contains(" APPLY "))
                    {
                        q.AppendFormat(" {0} {1} {2} ", j.tipo, j.relacion, j.tabla.alias);
                    }
                    else
                    {
                        q.AppendFormat(" {0} JOIN {1} {2} ON {3} ", j.tipo, j.tabla.nombre, j.tabla.alias, j.relacion);
                    }
                }
            }

            // Se agregan las condicionales en caso de existir
            if (this.wheres != null && this.wheres.Count > 0)
            {
                q.Append(" WHERE ");
                if (this.wheres != null && this.wheres.Count > 0)
                {
                    i = 0;
                    List<String> busqueda = new List<String> { "(", "AND", "SELECT" };
                    foreach (var w in this.wheres)
                    {
                        if (i > 0) q.Append(" AND ");

                        string valor;
                        // Se identifican las condicionales que requieren escaparse en apóstrofes
                        if (new List<String> { Where.BETWEEN, Where.NOTBETWEEN, Where.IN, Where.NOTIN, Where.EXISTS }.Contains(w.operador) || busqueda.Any(s => w.operador.ToUpper().Contains(s)))
                        {
                            valor = w.valor;
                        }
                        else
                        {
                            valor = "'" + w.valor + "'";
                        }

                        q.AppendFormat("{0} {1} {2}", w.columna, w.operador, valor);
                        i++;
                    }
                }
            }

            // Se agrega la agrupación
            if (this.groupBy != null) { q.Append(" GROUP BY  " + this.groupBy); }
            // Se agrega el ordenamiento
            if (this.orderBy != null) { q.Append(" ORDER BY  " + this.orderBy); }
            q.Append(")soft");
            return q.ToString();
        }
    }
}