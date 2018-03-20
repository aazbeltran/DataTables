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
    /// Representa una relación de tablas en la consulta.
    /// </summary>
    public class Join
    {
        /// <summary>
        /// Tipo de relación.
        /// </summary>
        public string tipo { get; set; }

        /// <summary>
        /// Tabla a relacionar.
        /// </summary>
        public Table tabla { get; set; }

        /// <summary>
        /// Condicionales de relación.
        /// </summary>
        public string relacion { get; set; }

        /// <summary>
        /// Crea una nueva instancia de Join
        /// con las propiedades como parámetros.
        /// </summary>
        /// <param name="tabla">Tabla a relacionar.</param>
        /// <param name="relacion">Condicionales de la relación.</param>
        /// <param name="tipo">Tipo de relación.</param>
        public Join(Table tabla, string relacion, string tipo = "LEFT")
        {
            this.tabla = tabla;
            this.relacion = relacion;
            this.tipo = tipo;
        }

        /// <summary>
        /// Crea una nueva instancia de Join.
        /// </summary>
        public Join()
        {
        }
    }
}