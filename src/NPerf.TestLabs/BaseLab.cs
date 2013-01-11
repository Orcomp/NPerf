using DaveSexton.Labs;

namespace NPerf.TestLabs
{
    /// <summary>
    /// Labs derived from this class may define a custom graphical user interface (GUI).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This base class is not typically used in .NET console applications because they 
    /// don't have graphical user interfaces.  It is included anyway for multi-platform 
    /// support only and may require conditional compilation, per lab, to convert custom 
    /// GUIs into console UIs.
    /// </para>
    /// <para>
    /// Note that this project does not compile to multiple target platforms.
    /// You must create a single labs project per platform and then add a Compile 
    /// link to this file in each project.  Conditional compilation is present to 
    /// make sure that the correct base class is used, depending upon the project.
    /// </para>
    /// </remarks>
    [System.ComponentModel.Composition.PartNotDiscoverable]
    public class BaseLab
#if WINDOWS_PHONE
 : PhoneLab
#elif SILVERLIGHT
 : SilverlightLab
#elif WINDOWS
 : WindowsLab
#else
 : Lab
#endif
    {
        protected BaseLab()
        {
        }
    }
}