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

namespace DT
{
    /// <summary>
    /// Representa una condición de la consulta.
    /// </summary>
    public class Where
    {
        /// <summary>
        /// Columna a aplicar condicional.
        /// </summary>
        public string columna { get; set; }

        /// <summary>
        /// Valor de la condicional.
        /// </summary>
        public string valor { get; set; }

        /// <summary>
        /// Apuntador del operador de la condicional.
        /// </summary>
        /// <value>La propiedad operador obtiene/establece el valor del campo string, _operador.</value>
        public string operador {
            get {
                return this._operador;
            }
            set {
                this._operador = value.ToUpper();
            }
        }

        /// <summary>
        /// Operador de la condicional.
        /// </summary>
        private string _operador = "=";

        /// <summary>
        /// Operador Igual que (=).
        /// </summary>
        public const string EQUALS = "=";

        /// <summary>
        /// Operador Diferente que (&lt;&gt;).
        /// </summary>
        public const string NOTEQUALS = "<>";

        /// <summary>
        /// Operador Menor que (&lt;).
        /// </summary>
        public const string LESS = "<";

        /// <summary>
        /// Operador Mayor que (&gt;).
        /// </summary>
        public const string GREATER = ">";

        /// <summary>
        /// Operador Menor o igual que (&lt;=).
        /// </summary>
        public const string LESSEQUAL = "<=";

        /// <summary>
        /// Operador Mayor o igual que (&gt;=).
        /// </summary>
        public const string GREATEREQUAL = ">=";

        /// <summary>
        /// Operador Entre (BETWEEN).
        /// </summary>
        public const string BETWEEN = "BETWEEN";

        /// <summary>
        /// Operador No entre (NOT BETWEEN).
        /// </summary>
        public const string NOTBETWEEN = "NOT BETWEEN";

        /// <summary>
        /// Operador Similar (LIKE).
        /// </summary>
        public const string LIKE = "LIKE";

        /// <summary>
        /// Operador No similar (NOT LIKE).
        /// </summary>
        public const string NOTLIKE = "NOT LIKE";

        /// <summary>
        /// Operador En (IN).
        /// </summary>
        public const string IN = "IN";

        /// <summary>
        /// Operador No en (NOT IN).
        /// </summary>
        public const string NOTIN = "NOT IN";

        /// <summary>
        /// Operador Existente (EXISTS).
        /// </summary>
        public const string EXISTS = "EXISTS";

        /// <summary>
        /// Crea una nueva instancia de Where
        /// con propiedades como parámetros.
        /// </summary>
        /// <param name="columna">Columna a aplicar condicional.</param>
        /// <param name="valor">Valor de la condicional.</param>
        /// <example>
        /// En este ejemplo se muestra como utilizar Where sin operador.
        /// <code>
        /// new Where("Activo", "1");
        /// </code>
        /// </example>
        public Where(string columna, string valor)
        {
            this.columna = columna;
            this.valor = valor;
        }

        /// <summary>
        /// Crea una nueva instancia de Where
        /// con propiedades como parámetros.
        /// </summary>
        /// <param name="columna">Columna a aplicar condicional.</param>
        /// <param name="operador">Operador de la condicional.</param>
        /// <param name="valor">Valor de la condicional.</param>
        /// <example>
        /// En estos ejemplos se muestra como utilizar Where con operador.
        /// <code>
        /// new Where("Lapso", Where.BETWEEN, "1 AND 3");
        /// new Where("Nombre", Where.LIKE, "%X%");
        /// new Where("Id", Where.IN, "(SELECT 1)");
        /// </code>
        /// </example>
        public Where(string columna, string operador, string valor)
        {
            this.columna = columna;
            this.operador = operador;
            this.valor = valor;
        }

        /// <summary>
        /// Crea una nueva instancia de Where
        /// con propiedades como parámetros.
        /// </summary>
        /// <param name="columna">Columna a aplicar condicional.</param>
        /// <param name="operador">Operador de la condicional.</param>
        /// <param name="valor">Instancia de Query con subconsulta como valor.</param>
        /// <example>
        /// En estos ejemplos se muestra como utilizar Where con una instancia de Query como valor.
        /// <code>
        /// new Where("", Where.EXISTS, new Query {
        ///     select = new List&lt;string&gt; {
        ///         "C.Id"
        ///     },
        ///     from = new Table("Sesiones", "S"),
        ///     where = new List&lt;Where&gt; {
        ///         new Where("S.IdUsuario", "parent.Id")
        ///     }
        /// });
        /// </code>
        /// </example>
        public Where(string columna, string operador, Query valor)
        {
            this.columna = columna;
            this.operador = operador;
            this.valor = "(" + valor.GenerarConsulta() + ")";
        }

        /// <summary>
        /// Crea una nueva instancia de Where.
        /// </summary>
        public Where()
        {
        }
    }
}