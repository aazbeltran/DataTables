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

namespace DT
{
    /// <summary>
    /// Representa la respuesta del DataTable.
    /// </summary>
    public class DataTable
    {
        /// <summary>
        /// El contador de solicitudes al que responde este objeto
        /// a partir del parámetro draw enviado como parte de la solicitud de datos.
        /// Tenga en cuenta que, por razones de seguridad,
        /// se recomienda encarecidamente lanzar este parámetro a un entero,
        /// en lugar de limitarse a hacer eco al cliente de lo que éste ha enviado en el parámetro de sorteo,
        /// para evitar ataques de XSS (Cross Site Scripting).
        /// </summary>
        public int draw { get; set; }

        /// <summary>
        /// Total de registros, antes de filtrar
        /// (es decir, el número total de registros en la base de datos).
        /// </summary>
        public int recordsTotal { get; set; }

        /// <summary>
        /// Total de registros, después del filtrado
        /// (es decir, el número total de registros después del filtrado que se ha aplicado,
        /// no sólo el número de registros que se devuelven para esta página de datos).
        /// </summary>
        public int recordsFiltered { get; set; }

        /// <summary>
        /// Los datos que se visualizarán en la tabla.
        /// Se trata de un array de objetos fuente de datos,
        /// uno por cada fila, que serán utilizados por DataTables.
        /// </summary>
        public List<Object> data { get; set; }

        /// <summary>
        /// Si se produce un error durante la ejecución
        /// del script de procesamiento del lado del servidor,
        /// puede informar al usuario de este error devolviendo
        /// el mensaje de error que se mostrará utilizando este parámetro.
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// Crea una nueva instancia de DataTable
        /// con propiedades como parámetros.
        /// </summary>
        /// <param name="draw">Contador de solicitud.</param>
        /// <param name="data">Registros a visualizar en la tabla.</param>
        /// <param name="recordsTotal">Total de registros en la consulta.</param>
        /// <param name="recordsFiltered">Total de registros en la consulta después del filtrado.</param>
        /// <param name="error">Error ocurrido durante la ejecución.</param>
        public DataTable(int draw = 0, List<Object> data = null, int recordsTotal = 0, int recordsFiltered = 0, string error = null)
        {
            this.draw = draw;
            this.data = data ?? new List<Object>();
            this.recordsTotal = recordsTotal;
            this.recordsFiltered = recordsFiltered;
            this.error = error;
        }

        /// <summary>
        /// Crea una nueva instancia de DataTable.
        /// </summary>
        public DataTable()
        {
        }
    }
}