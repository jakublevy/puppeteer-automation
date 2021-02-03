namespace Frontend
{
    /// <summary>
    /// Default configuration of things that are related to Node.js.
    /// </summary>
    public class NodeJsOptions
    {
        /// <summary>
        /// The value "node" supposes that Node.js interpreter is in environment variable PATH.
        /// </summary>
        public string InterpreterPath { get; set; } = "node";

        /// <summary>
        /// Relative path from Frontend executable to Backend entry point file.
        /// This path is valid if the project structure is not changed.
        /// </summary>
        public string NodeJsEntryPoint { get; set; } = "..\\..\\..\\..\\Backend\\src\\main.js";

    }
}
