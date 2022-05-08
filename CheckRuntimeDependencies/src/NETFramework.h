#pragma once

#include <string>

namespace RuntimeCheck {

	class NETFramework
	{
	public:
		enum Version : int {
			NET_4_8 = 528372,
			NET_4_7_2 = 461808,
			NET_4_7_1 = 461308,
			NET_4_7 = 460798,
			NET_4_6_2 = 394802,
			NET_4_6_1 = 394254,
			NET_4_6 = 393295,
			NET_4_5_2 = 379893,
			NET_4_5_1 = 378675,
			NET_4_5 = 378389
		};

		void SetTargetedVersion(Version targetedVersion) { _version = targetedVersion; }
		bool CheckIfVersionIsInstalled();

	private:
		int _version;

		void DisplayNotInstalledMessageBox();
		std::string GetVersionString();
	};

}