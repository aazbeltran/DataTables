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
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DT
{
    /// <summary>
    /// Representa la clase funcional de DataTables.
    /// </summary>
    public class DataTables
    {
        /// <summary>
        /// Conexión a la base de datos.
        /// </summary>
        private DbConnection db;

        /// <summary>
        /// Tipo de la base de datos.
        /// </summary>
        private string dbEngine;

        /// <summary>
        /// Crea una nueva instancia de DataTables
        /// con propiedades como parámetros.
        /// </summary>
        /// <param name="db">Conexión a la base de datos.</param>
        public DataTables(DbConnection db)
        {
            this.db = db;
            // Se obtiene el motor de base de datos y se asigna
            this.dbEngine = db.GetType().Name.Replace("Connection", "");
        }

        /// <summary>
        /// Genera un nuevo DataTable.
        /// </summary>
        /// <param name="query">Instancia de Query con la consulta inicial.</param>
        /// <param name="draw">Contador de la solicitud.</param>
        /// <param name="start">Registro inicial para el paginado.</param>
        /// <param name="length">Tamaño del paginado.</param>
        /// <param name="search">Instancia de Search con la búsqueda general.</param>
        /// <param name="orders">Lista de instrucciones de ordenamiento.</param>
        /// <param name="columns">Columnas del DataTable.</param>
        /// <returns>Retorna una instancia de DataTable <see cref="DataTable"/></returns>
        public DataTable Generar(Query query, int draw, int start, int length, Search search, List<Order> orders, List<Column> columns)
        {
            try
            {
                // Se genera la consulta inicial
                string consulta = query.GenerarConsulta();
                // Se agrega la búsqueda general
                string consultaBusqueda = AgregarBusqueda(consulta, query.select, search);
                // Se agrega el paginado
                string consultaLimit = AgregarLimit(consultaBusqueda, start, length, orders, columns);

                // Se obtienen los totales
                Totals totals = ObtenerTotals(consulta, consultaBusqueda);

                // Se obtiene el resultado de la consulta
                List<Object> result = ExecutarConsulta(consultaLimit, query.select);

                // Se crea y retorna el DataTable formateado
                return new DataTable(draw, result, totals.total, totals.filtered);
            }
            catch (Exception ex)
            {
                // Si ocurre un error se devuelve una tabla vacía con el error
                return DataTables.ObtenerDataTableVacia(ex.ToString(), draw);
            }
        }

        /// <summary>
        /// Genera un nuevo DataTable.
        /// </summary>
        /// <param name="queries">Lista de Instancias de Query con la consulta inicial.</param>
        /// <param name="draw">Contador de la solicitud.</param>
        /// <param name="start">Registro inicial para el paginado.</param>
        /// <param name="length">Tamaño del paginado.</param>
        /// <param name="search">Instancia de Search con la búsqueda general.</param>
        /// <param name="orders">Lista de instrucciones de ordenamiento.</param>
        /// <param name="columns">Columnas del DataTable.</param>
        /// <returns>Retorna una instancia de DataTable <see cref="DataTable"/></returns>
        public DataTable Generar(List<Query> queries, int draw, int start, int length, Search search, List<Order> orders, List<Column> columns)
        {
            try
            {
                StringBuilder query = new StringBuilder("");
                var i = 0;
                // Se unen todas las consultas
                foreach (var q in queries)
                {
                    if (i > 0) query.Append(" UNION ALL ");
                    query.Append(q.GenerarConsulta());
                    i++;
                }
                // Se genera la consulta inicial
                string consulta = query.ToString();
                // Se agrega la búsqueda general
                string consultaBusqueda = AgregarBusqueda(consulta, queries[0].select, search);
                // Se agrega el paginado
                string consultaLimit = AgregarLimit(consultaBusqueda, start, length, orders, columns);

                // Se obtienen los totales
                Totals totals = ObtenerTotals(consulta, consultaBusqueda);

                // Se obtiene el resultado de la consulta
                List<Object> result = ExecutarConsulta(consultaLimit, queries[0].select);

                // Se crea y retorna el DataTable formateado
                return new DataTable(draw, result, totals.total, totals.filtered);
            }
            catch (Exception ex)
            {
                // Si ocurre un error se devuelve una tabla vacía con el error
                return DataTables.ObtenerDataTableVacia(ex.ToString(), draw);
            }
        }

        /// <summary>
        /// Genera un DataTable vacío.
        /// </summary>
        /// <param name="error">Error ocurrido en el proceso.</param>
        /// <param name="draw">Contador de la solicitud.</param>
        /// <returns>Retorna una instancia vacía de DataTable <see cref="DataTable"/></returns>
        public static DataTable ObtenerDataTableVacia(string error, int draw)
        {
            return new DataTable(draw, error: error);
        }

        /// <summary>
        /// Agrega la búsqueda general.
        /// </summary>
        /// <param name="query">Consulta inicial.</param>
        /// <param name="select">Lista de columnas a visualizar.</param>
        /// <param name="search">Instancia de Search con la búsqueda general.</param>
        /// <returns>Retorna la consulta con la búsqueda.</returns>
        private string AgregarBusqueda(string query, List<string> select, Search search)
        {
            int i;
            StringBuilder q = new StringBuilder(query);
            if (!String.IsNullOrEmpty(search.value))
            {
                q.Append(" WHERE ");
                i = 0;
                List<String> omitidos = new List<String> { "Lectura", "Escritura", "Eliminar" };
                foreach (var campo in select)
                {
                    // Se busca en todos los campos que no estén en los omitidos
                    if (!omitidos.Any(s => campo.Contains(s)))
                    {
                        // Se obtiene la columna real a utilizar
                        string column = sanitizeColumn(campo);
                        q.AppendFormat(" {0} {1} LIKE '%{2}%'", i == 0 ? "" : "OR", column, search.value);
                        i++;
                    }
                }
            }
            return q.ToString();
        }

        /// <summary>
        /// Agrega el paginado a la consulta.
        /// </summary>
        /// <param name="query">Consulta actual.</param>
        /// <param name="start">Registro inicial para el paginado.</param>
        /// <param name="length">Tamaño de paginado.</param>
        /// <param name="orders">Lista de instrucciones de ordenamiento.</param>
        /// <param name="columns">Columnas del DataTable.</param>
        /// <returns>Retorna la consulta con el paginado</returns>
        private string AgregarLimit(string query, int start, int length, List<Order> orders, List<Column> columns)
        {
            StringBuilder q = new StringBuilder("");
            StringBuilder query_order = new StringBuilder("");
            int i = 0;
            // Se recorre la lista de ordenamientos
            foreach (var o in orders)
            {
                // Se obtiene la columna real a utilizar
                string column = sanitizeColumn(columns[o.column].name);
                query_order.AppendFormat("{0}{1} {2}", i > 0 ? "," : "", column, o.dir);
                i++;
            }

            string order = query_order.ToString() != "" ? query_order.ToString() : sanitizeColumn(columns[0].name) + " ASC";
            // Dependiento del motor de base de datos utilizado, realiza el paginado de la manera adecuada
            switch (dbEngine)
            {
                case "MySql":
                    q.Append(query);
                    q.AppendFormat(" ORDER BY {0} ", order);
                    q.AppendFormat(" LIMIT {0},{1} ", start, length);
                    break;

                case "Sql":
                    q.AppendFormat("WITH DataTable AS (SELECT ROW_NUMBER() OVER (ORDER BY {0}) RowNumber,* FROM (", order);
                    q.Append(query);
                    q.AppendFormat(")) SELECT * FROM DataTable WHERE RowNumber BETWEEN {0} AND {1}", start + 1, start + length);
                    break;

                default:
                    throw new Exception("DataTables no está programado para trabajar con esta conexión de base de datos.");
            }
            return q.ToString();
        }

        /// <summary>
        /// Se ejecuta la consulta final.
        /// </summary>
        /// <param name="query">Consulta final.</param>
        /// <param name="select">Lista de columnas a visualizar.</param>
        /// <returns>Retorna la consulta con el paginado</returns>
        private List<Object> ExecutarConsulta(string query, List<string> select)
        {
            // Se crea un comando de la conexión actual de la base de datos
            DbCommand command = this.db.CreateCommand();
            command.CommandTimeout = 1000;
            command.CommandType = System.Data.CommandType.Text;
            // Se prepara el comando para ejecutar la consulta
            command.CommandText = query;
            DbDataReader dr;

            this.db.Open();
            dr = command.ExecuteReader();
            List<Object> Resultado = new List<Object>();
            using (dr)
            {
                while (dr.Read())
                {
                    // Se utiliza ExpandoObject como diccionario
                    var obj = new System.Dynamic.ExpandoObject() as IDictionary<string, Object>;
                    foreach (string col in select)
                    {
                        string col_c = "";
                        // Se obtiene la propiedad y valor
                        if (col.Contains("+") || col.Contains("-") || col.Contains("*") || col.Contains("/") || col.Contains("stuff((") || col.Contains("(SELECT "))
                        {
                            if (col.Contains("stuff((") || col.Contains("(SELECT "))
                            {
                                string[] split_col_c = col.Split(' ');
                                col_c = split_col_c[split_col_c.Length - 1];
                            }
                            else if (col.Contains(" "))
                            {
                                string[] split_col_c = col.Split(' ');
                                col_c = split_col_c[1];
                            }
                        }
                        else
                        {
                            string[] split = col.Split('.');
                            col_c = split[split.Length == 1 ? 0 : 1];
                            if (col_c.Contains(" "))
                            {
                                string[] split_col_c = col_c.Split(' ');
                                col_c = split_col_c[1];
                            }
                        }
                        // Se agrega la propiedad al objeto
                        obj.Add(col_c, dr[col_c]);
                    }
                    // Se corrige el serializado del objeto
                    var tObj = obj.ToDictionary(x => x.Key, x => x.Value);
                    // Se agrega el registro al listado de resultados
                    Resultado.Add(tObj);
                }
            }
            this.db.Close();
            return Resultado;
        }

        /// <summary>
        /// Se obtienen los totales de la consulta.
        /// </summary>
        /// <param name="consultaTotal">Consulta del total de registros.</param>
        /// <param name="consultaFiltered">Consulta del total de registros filtrada.</param>
        /// <returns>Retorna los totales de registros</returns>
        private Totals ObtenerTotals(string consultaTotal, string consultaFiltered)
        {
            // Se crea un comando de la conexión actual de la base de datos
            DbCommand command = this.db.CreateCommand();
            command.CommandTimeout = 1000;
            command.CommandType = System.Data.CommandType.Text;
            // Se prepara el comando para ejecutar la consulta
            command.CommandText = "SELECT (SELECT COUNT(*) FROM(" + consultaTotal + ")a)Total, (SELECT COUNT(*) FROM(" + consultaFiltered + ")b)Filtered";
            DbDataReader dr;

            this.db.Open();
            dr = command.ExecuteReader();
            Totals totals = new Totals();
            using (dr)
            {
                if (dr.Read())
                {
                    totals.total = Int32.Parse(dr["Total"].ToString());
                    totals.filtered = Int32.Parse(dr["Filtered"].ToString());
                }
            }
            this.db.Close();
            return totals;
        }

        /// <summary>
        /// Se obtiene el la columna real a consultar.
        /// </summary>
        /// <param name="campo">Columna a sanitizar</param>
        /// <returns>Retorna la columna sanitizada</returns>
        private string sanitizeColumn(string campo)
        {
            string column;
            // Si contiene espacios, se hace partición.
            if (campo.Contains(" "))
            {
                string[] split = campo.Split(' ');
                if (campo.Contains("stuff(("))
                {
                    string campostuff = string.Empty;
                    for (int indexstuff = 0; indexstuff < (split.Length - 1); indexstuff++)
                    {
                        campostuff = string.Concat(campostuff, " ", split[indexstuff]);
                    }
                    column = campostuff;
                }
                else
                {
                    column = split[0];
                }
            }
            else
            {
                column = campo;
            }
            // Si se utiliza un alias o referenciación entera, se obtiene la ultima propiedad accedida (la columna)
            string[] splitColumn = column.Split('.');
            return splitColumn[splitColumn.Count() - 1];
        }
    }
}