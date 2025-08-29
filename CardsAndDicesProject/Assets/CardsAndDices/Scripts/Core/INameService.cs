namespace CardsAndDices
{
    /// <summary>
    /// Defines the behavior for a name resolution service.
    /// </summary>
    public interface INameService
    {
        /// <summary>
        /// Gets the display name for a given entity definition.
        /// </summary>
        /// <param name="entityDef">The entity definition.</param>
        /// <returns>The display name string.</returns>
        string GetDisplayName(BaseEntityDefinition entityDef);

        /// <summary>
        /// Gets the description for a given entity definition.
        /// </summary>
        /// <param name="entityDef">The entity definition.</param>
        /// <returns>The description string.</returns>
        string GetDescription(BaseEntityDefinition entityDef);
    }
}
