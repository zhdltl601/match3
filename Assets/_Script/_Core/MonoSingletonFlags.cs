using System;

[Flags]
public enum MonoSingletonFlags
{
    None            	    = 0,
    DontDestroyOnLoad       = 1 << 0,
    SingletonPreset         = 1 << 1,
    Hide                    = 1 << 2
}