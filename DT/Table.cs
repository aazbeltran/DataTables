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
    /// Representa una tabla en la consulta.
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Nombre de la tabla.
        /// </summary>
        public string nombre { get; set; }

        /// <summary>
        /// Alias utilizado para la tabla.
        /// </summary>
        public string alias { get; set; }

        /// <summary>
        /// Crea una nueva instancia de Table
        /// con propiedades como parámetros.
        /// </summary>
        /// <param name="nombre">Nombre de la tabla.</param>
        public Table(string nombre)
        {
            this.nombre = nombre;
        }

        /// <summary>
        /// Crea una nueva instancia de Table
        /// con propiedades como parámetros.
        /// </summary>
        /// <param name="nombre">Nombre de la tabla.</param>
        /// <param name="alias">Alias utilizado para la tabla.</param>
        public Table(string nombre, string alias)
        {
            this.nombre = nombre;
            this.alias = alias;
        }

        /// <summary>
        /// Crea una nueva instancia de Table.
        /// </summary>
        public Table()
        {
        }
    }
}