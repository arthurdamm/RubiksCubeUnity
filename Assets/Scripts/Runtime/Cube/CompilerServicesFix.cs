using System.ComponentModel;

// IsExternalInit definition is necessary to allow "init" prop accessors in Unity C#
// https://docs.unity3d.com/6000.3/Documentation/Manual/csharp-compiler.html
namespace System.Runtime.CompilerServices
{
[EditorBrowsable(EditorBrowsableState.Never)]
internal class IsExternalInit {}
}
