using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kok
{
    [CreateAssetMenu (menuName = "App Version")]
    public class AppVersion : ScriptableObject
    {
		[Header ("Semantic Version")]
		[Min (0)] [SerializeField] private int major = 0;
		[Min (0)] [SerializeField] private int minor = 0;
		[Min (0)] [SerializeField] private int patch = 0;
		[Header ("Release Suffix")]
		[SerializeField] private ReleaseType release = ReleaseType.Development;
		[Min (0)] [SerializeField] private int releaseUpdate = 0;

		protected virtual string GetVersionNumber ()
		{
			string releaseSuffix = "";

			switch (release)
			{
				case ReleaseType.Development:
					releaseSuffix = "-dev";
					break;
				case ReleaseType.Alpha:
					releaseSuffix = "-a" + releaseUpdate.ToString ();
					break;
				case ReleaseType.Beta:
					releaseSuffix = "-b" + releaseUpdate.ToString ();
					break;
				default:
					break;
			}

			return string.Format ("{0}.{1}.{2}{3}", major, minor, patch, releaseSuffix);
		}

		public static implicit operator string (AppVersion version)
		{
			return version.GetVersionNumber ();
		}

		public override string ToString ()
		{
			return GetVersionNumber ();
		}
	}

	public enum ReleaseType
	{
		Development,
		Alpha,
		Beta,
		Production
	}
}