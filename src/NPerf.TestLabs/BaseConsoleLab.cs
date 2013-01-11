using DaveSexton.Labs;

namespace NPerf.TestLabs
{
    /// <summary>
    /// Labs derived from this class have a console user interface (UI).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Call the inherited Trace methods to write output to the console.
    /// </para>
    /// <para>
    /// Note that this project does not compile to multiple target platforms.
    /// You must create a single labs project per platform and then add a Compile 
    /// link to this file in each project.  Conditional compilation is present to 
    /// make sure that the correct base class is used, depending upon the project.
    /// </para>
    /// </remarks>
    public abstract class BaseConsoleLab
#if WINDOWS_PHONE
 : PhoneConsoleLab
#elif SILVERLIGHT
 : SilverlightConsoleLab
#elif WINDOWS
 : WindowsConsoleLab
#else
 : ConsoleLab
#endif
    {
        protected BaseConsoleLab()
        {
        }
    }
}