# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="6.0.0"></a>
## [6.0.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/6.0.0) (2026-11-27)

### ✨ Features

* migrate application to .NET 11 ([f247f89](https://www.github.com/mu88/ScreenshotCreator/commit/f247f894dd884058596b00c60f31d50f91ad7a15))
* migrate code coverage from coverlet to dotnet-coverage ([f19b8da](https://www.github.com/mu88/ScreenshotCreator/commit/f19b8dab2e5548aa331e17da2ae3f7d167c476e7))
* **renovate:** add timestamp-optional behavior for specific packages to ensure updates are not skipped ([2a1c37e](https://www.github.com/mu88/ScreenshotCreator/commit/2a1c37e1dfa5d1bfc0b07b7eace32a03b75be06a))

### 🐛 Bug Fixes

* bump ContainerBaseImage to pwsh-free Playwright base image ([b812477](https://www.github.com/mu88/ScreenshotCreator/commit/b81247705aecfbc78a43b4edea073a2a37034504))
* remove pwsh from runtime image after Playwright install ([7834407](https://www.github.com/mu88/ScreenshotCreator/commit/7834407e9150efa1d1104931f6de5c46b684afba))
* resolve SonarCloud findings (merge RUN layers, mark test helper static) ([ed436db](https://www.github.com/mu88/ScreenshotCreator/commit/ed436dbd22c263d985f987117291b65653b37cce))
* update Magick.NET-Q16-AnyCPU to version 14.10.0 across all projects and hereby fix security vulnerability ([c81a301](https://www.github.com/mu88/ScreenshotCreator/commit/c81a301b722eaaffa729de81780d779f28a3b3dd))
* use latest version of mu88.Shared to fix some OTEl warnings ([9859e62](https://www.github.com/mu88/ScreenshotCreator/commit/9859e62cdd43be9863a8b85ca6e9869ed5630df8))
* **ci:** track temporary webutilities pin ([176901d](https://www.github.com/mu88/ScreenshotCreator/commit/176901d4d00d1a86f8a111df0a607249c2fb384a))
* **container:** bump Playwright base image to aspnet 10.0.13 ([0b3839a](https://www.github.com/mu88/ScreenshotCreator/commit/0b3839aa4e170230c691ff8e37ead6f791ae5767))
* **container:** reference patched Playwright base image 10.0.261 ([38641ff](https://www.github.com/mu88/ScreenshotCreator/commit/38641ffa367447fcb0d1f9334a66f0c6f5bc2806))
* **container:** verify SHA256 checksum of downloaded PowerShell arm64 tarball ([d5fff30](https://www.github.com/mu88/ScreenshotCreator/commit/d5fff30bdf1ad67bb0e33b6400cd875b808724a8))
* **deps:** adopt shared testing publish worker fix ([7faf258](https://www.github.com/mu88/ScreenshotCreator/commit/7faf258cf4f07266c46eca5eeac8cbc8981666b1))
* **deps:** migrate Magick.NET 15 alpha API ([20ace66](https://www.github.com/mu88/ScreenshotCreator/commit/20ace661fa3eab4f661e03ed86ccaf347ada82d3))
* **security:** patch runtime and test dependency ([997bda0](https://www.github.com/mu88/ScreenshotCreator/commit/997bda0ba5d361f543b492e815c57c4ea2f5bba8))
* **tests:** use valid SemVer for system test container image tag ([c35d87d](https://www.github.com/mu88/ScreenshotCreator/commit/c35d87d7d934498eb555bb72143334d4b340bb05))

### ♻️ Refactors

* encapsulate Logic internals and apply code review findings ([85d6c4a](https://www.github.com/mu88/ScreenshotCreator/commit/85d6c4af32e17f62a6ff9e8862bec3bafed017a2))
* **ci:** use centralized pin guard ([59bc808](https://www.github.com/mu88/ScreenshotCreator/commit/59bc8084febee2aab4e1c6a358747e030a77b6aa))
* **tests:** adopt mu88.Shared.Testing for System test infrastructure ([68cd66b](https://www.github.com/mu88/ScreenshotCreator/commit/68cd66b462cee6851cdc9d29b536ddf7f9700c8c))

### ✅ Tests

* improve stability by properly waiting asynchronously in Playwright ([ef8c90d](https://www.github.com/mu88/ScreenshotCreator/commit/ef8c90ddef90aabcbf20657091d685d0e2fc3b82))
* improve system test ([869cf5f](https://www.github.com/mu88/ScreenshotCreator/commit/869cf5f1ea6ec99c7b8ea7dd863e5e04edc65e02))
* simplify build process by using CliWrap ([319811a](https://www.github.com/mu88/ScreenshotCreator/commit/319811a9282ad9ed2535a2f3b77ff14db0813053))

### 🔧 Chores

* apply new code style ([c539881](https://www.github.com/mu88/ScreenshotCreator/commit/c5398812bf9f5c56e9881d632f601f9a006865b8))
* resolve Sonar issues and push test coverage ([51361c0](https://www.github.com/mu88/ScreenshotCreator/commit/51361c07519fc225f6286e615668f3b8c7a410fb))
* use new shared GitHub repo mu88/common ([855fea4](https://www.github.com/mu88/ScreenshotCreator/commit/855fea44b720b0fde213e27e49f722d718de7d40))
* **deps:** bump Microsoft.Extensions.Logging.Abstractions and Options to 10.0.11 ([58b7947](https://www.github.com/mu88/ScreenshotCreator/commit/58b7947a528b3f66354dbde4469c23dc59dfa9a0))
* **deps:** resolve security vulnerability by upgrading `Magick.NET` ([96b6806](https://www.github.com/mu88/ScreenshotCreator/commit/96b6806ede0c0e2970d6ec88ad6a00dd52485a44))
* **deps:** resolve security vulnerability by upgrading `Magick.NET` ([c0fef83](https://www.github.com/mu88/ScreenshotCreator/commit/c0fef83d09aa7fc714d314e7b2a61215f060e365))
* **deps:** update all .net ([662cc44](https://www.github.com/mu88/ScreenshotCreator/commit/662cc441180a5a4ace00d57ee8dd0551197d7cab))
* **deps:** update all .net ([eb8f224](https://www.github.com/mu88/ScreenshotCreator/commit/eb8f2244f72b1a2e2358f80684ad50123824067b))
* **deps:** update all .net ([eb4fd86](https://www.github.com/mu88/ScreenshotCreator/commit/eb4fd8603c60dbb819a9a55b239517ce14155afc))
* **deps:** update all .net ([c1c4b69](https://www.github.com/mu88/ScreenshotCreator/commit/c1c4b690a518abdea356ffe7dccc273c90116630))
* **deps:** update all .net ([f69f8d0](https://www.github.com/mu88/ScreenshotCreator/commit/f69f8d0c82ba469e9b17a5e5359ad35522c3156c))
* **deps:** update all .net ([955667a](https://www.github.com/mu88/ScreenshotCreator/commit/955667aaf882cf99fde34982b94bd19fbcf9f57a))
* **deps:** update all .net ([f107ce2](https://www.github.com/mu88/ScreenshotCreator/commit/f107ce2fb686af621c23c04251e722fff1ed4dc6))
* **deps:** update all .net ([22b52a0](https://www.github.com/mu88/ScreenshotCreator/commit/22b52a093d0c38439cf3491a5c9ce4464e9e8d90))
* **deps:** update all .net ([99735d5](https://www.github.com/mu88/ScreenshotCreator/commit/99735d5d98c35c34185d5faf76079e179a9826b4))
* **deps:** update all .net ([61ed669](https://www.github.com/mu88/ScreenshotCreator/commit/61ed66938843a75e705716eb88d1708f5c4a1dbe))
* **deps:** update all dependencies ([7c5b810](https://www.github.com/mu88/ScreenshotCreator/commit/7c5b810cafeb189e13604b56943ec7b37d0ad496))
* **deps:** update all dependencies ([d1939c1](https://www.github.com/mu88/ScreenshotCreator/commit/d1939c127cc4159476c01efb71df4cfd89f65611))
* **deps:** update all dependencies ([721b2d1](https://www.github.com/mu88/ScreenshotCreator/commit/721b2d15e8d63e44a1dfb7c22d54e672b2ce7e66))
* **deps:** update all dependencies ([79e1370](https://www.github.com/mu88/ScreenshotCreator/commit/79e1370b88beda28e937e16701b4dda500a03104))
* **deps:** update all dependencies ([2894241](https://www.github.com/mu88/ScreenshotCreator/commit/289424176f23e71e4cc0f2d0fff64f63d42130fe))
* **deps:** update all dependencies ([a7e8854](https://www.github.com/mu88/ScreenshotCreator/commit/a7e8854dc3e1cfccd0c1e992372ba0990814b54d))
* **deps:** update all dependencies ([9a29ff0](https://www.github.com/mu88/ScreenshotCreator/commit/9a29ff0a9925659afc01fb4eda3eee32b7a449b6))
* **deps:** update all dependencies ([2fca1e9](https://www.github.com/mu88/ScreenshotCreator/commit/2fca1e9b414cdd0baba2d6011fa3d0259788127c))
* **deps:** update all dependencies ([a0bc501](https://www.github.com/mu88/ScreenshotCreator/commit/a0bc5018969a907850e0bfc7c52c5cf6fb3a3164))
* **deps:** update all dependencies ([2294ced](https://www.github.com/mu88/ScreenshotCreator/commit/2294ced2c60d8e6b76e867964fe09afea2add99c))
* **deps:** update all dependencies ([fb85cef](https://www.github.com/mu88/ScreenshotCreator/commit/fb85cef317205538e9911eac2384ba7b6af92252))
* **deps:** update all dependencies ([9b47d9b](https://www.github.com/mu88/ScreenshotCreator/commit/9b47d9b09c37670ab78c72e2d545ccfabb3dc333))
* **deps:** update all dependencies ([167d0d9](https://www.github.com/mu88/ScreenshotCreator/commit/167d0d9240360357090a63721aac55e1cdf13528))
* **deps:** update all dependencies ([6a4f599](https://www.github.com/mu88/ScreenshotCreator/commit/6a4f599362887a84b21f6a277b8509611b3d96f9))
* **deps:** update all dependencies ([1342193](https://www.github.com/mu88/ScreenshotCreator/commit/1342193c10c47ace57534e5cb22f5b55c6fb97e2))
* **deps:** update all dependencies ([9a33cf9](https://www.github.com/mu88/ScreenshotCreator/commit/9a33cf93607b1506980fc0c6c6eae44a67eda2eb))
* **deps:** update all dependencies ([db059a9](https://www.github.com/mu88/ScreenshotCreator/commit/db059a92ec1dbeecd504f700141b68d9509a1146))
* **deps:** update all dependencies ([2d66fbd](https://www.github.com/mu88/ScreenshotCreator/commit/2d66fbd04278c6aa32f6ebde97948e462c2ca088))
* **deps:** update all dependencies ([243bfe0](https://www.github.com/mu88/ScreenshotCreator/commit/243bfe0b3ee0cd648f3603d8066d6f6e1005c07f))
* **deps:** update all dependencies ([851119f](https://www.github.com/mu88/ScreenshotCreator/commit/851119f8cda5b46290312cc701f613424d0a473a))
* **deps:** update all dependencies ([9fdd308](https://www.github.com/mu88/ScreenshotCreator/commit/9fdd308dc7c0f85b9acc6ca0ebe6623d3bc46e57))
* **deps:** update all dependencies ([35a7c5d](https://www.github.com/mu88/ScreenshotCreator/commit/35a7c5da7970cd141c6ec8c9a91731684a95b6d3))
* **deps:** update all dependencies ([1a01bd5](https://www.github.com/mu88/ScreenshotCreator/commit/1a01bd5654dbd7c42fc578e2409b2dfc1499bc0f))
* **deps:** update all dependencies ([95e57ad](https://www.github.com/mu88/ScreenshotCreator/commit/95e57ada5694a16c77060422e59bf30db62f8be4))
* **deps:** update all dependencies ([df8c336](https://www.github.com/mu88/ScreenshotCreator/commit/df8c336bbedb11acc378220cfd28db8d285f7928))
* **deps:** update all dependencies ([e488e7d](https://www.github.com/mu88/ScreenshotCreator/commit/e488e7dc0f63a81275a908cfb2e1726d33ba0060))
* **deps:** update all dependencies ([6624fb9](https://www.github.com/mu88/ScreenshotCreator/commit/6624fb992b87ca5b3b6388a839bbfccb68140fdd))
* **deps:** update all dependencies ([59e784e](https://www.github.com/mu88/ScreenshotCreator/commit/59e784ecb1c81fddc044cdf0fce25c784ed8516b))
* **deps:** update all dependencies to 10.0.2 ([9530e43](https://www.github.com/mu88/ScreenshotCreator/commit/9530e437dfc7cffdb26b5541174a641ac6a9afd9))
* **deps:** update all dependencies to v18.8.0 ([476f950](https://www.github.com/mu88/ScreenshotCreator/commit/476f950f016e1ec5794742e2effddeba460d696a))
* **deps:** update dependency dotnet-coverage to v18.12.0 ([eae9ab0](https://www.github.com/mu88/ScreenshotCreator/commit/eae9ab060d8359819c57ecf69bed7cef31b73e6f))
* **deps:** update dependency dotnet-coverage to v18.9.0 ([818ffe2](https://www.github.com/mu88/ScreenshotCreator/commit/818ffe28f2cc576ddd0bddd06f163f9f94a88d76))
* **deps:** update dependency fluentassertions to 8.11.0 ([5dfccb3](https://www.github.com/mu88/ScreenshotCreator/commit/5dfccb322c3034849a44fcfa5b8b78f81892b309))
* **deps:** update dependency magick.net-q16-anycpu to 14.11.0 [security] ([968c993](https://www.github.com/mu88/ScreenshotCreator/commit/968c99358b8af6e4241cb8347dbd658ff5e82585))
* **deps:** update dependency magick.net-q16-anycpu to 14.11.1 [security] ([6a6d612](https://www.github.com/mu88/ScreenshotCreator/commit/6a6d61239ca2626c95dd68776b75cceceae75640))
* **deps:** update dependency magick.net-q16-anycpu to 14.12.0 [security] ([54b180a](https://www.github.com/mu88/ScreenshotCreator/commit/54b180a096afda310701d8316ab2b144d4ee489f))
* **deps:** update dependency magick.net-q16-anycpu to 14.13.1 [security] ([cc7926b](https://www.github.com/mu88/ScreenshotCreator/commit/cc7926b916b598905e648c4e23675fc86ca6310f))
* **deps:** update dependency magick.net-q16-anycpu to 14.15.0 [security] ([1aa9d04](https://www.github.com/mu88/ScreenshotCreator/commit/1aa9d04a8823fc00f56acf7b773058a47b675eb9))
* **deps:** update dependency microsoft.playwright to 1.57.0 ([07a57e7](https://www.github.com/mu88/ScreenshotCreator/commit/07a57e7a9153eeeb2901fd3192546545316a215b))
* **deps:** update dependency microsoft.playwright to 1.58.0 ([5ed9442](https://www.github.com/mu88/ScreenshotCreator/commit/5ed9442c73f2b80f5fa0fc91090f6e64dd43a6c1))
* **deps:** update dependency microsoft.playwright to 1.61.0 ([eb2ce35](https://www.github.com/mu88/ScreenshotCreator/commit/eb2ce35df0c51e22154d14a592385e7f54d6fd32))
* **deps:** update dependency microsoft.playwright to 1.62.0 ([449fd8a](https://www.github.com/mu88/ScreenshotCreator/commit/449fd8a8d0301dc6d39737e7b095fa73c5d84aec))
* **deps:** update dependency microsoft.playwright to 1.63.0 ([1a356df](https://www.github.com/mu88/ScreenshotCreator/commit/1a356dfd422faeae731dbe0905bbcaf8c77346ea))
* **deps:** update dependency microsoft.playwright to 1.64.1 ([3f75806](https://www.github.com/mu88/ScreenshotCreator/commit/3f7580692e2b51f309ca2355420b265755d94216))
* **deps:** update dependency scalar.aspnetcore to 2.12.36 ([05ee7a1](https://www.github.com/mu88/ScreenshotCreator/commit/05ee7a1627ce2af47b10b46a48644f702d2e0453))
* **deps:** update dependency scalar.aspnetcore to 2.13.17 ([577995e](https://www.github.com/mu88/ScreenshotCreator/commit/577995e3222041c6b5eb4b927b3a0eed978feab2))
* **deps:** update dependency scalar.aspnetcore to 2.17.3 ([75f4250](https://www.github.com/mu88/ScreenshotCreator/commit/75f4250cdcacc01cd90a8b5ab64e0263448b7d82))
* **deps:** update ghcr.io/mu88/screenshotcreator-playwright docker tag to v10.0.130 ([4801063](https://www.github.com/mu88/ScreenshotCreator/commit/48010639e99f14d20b6d2b4bc86161c1e559b11a))
* **deps:** update ghcr.io/mu88/screenshotcreator-playwright docker tag to v10.0.184 ([0a4fef0](https://www.github.com/mu88/ScreenshotCreator/commit/0a4fef0b012a2cd30864de8d23592ef5d3686a9a))
* **deps:** update ghcr.io/mu88/screenshotcreator-playwright docker tag to v10.0.241 ([df913c0](https://www.github.com/mu88/ScreenshotCreator/commit/df913c0221b6004846e93cc25444af5cd21991dc))
* **deps:** update ghcr.io/mu88/screenshotcreator-playwright docker tag to v10.0.250 ([3e792a8](https://www.github.com/mu88/ScreenshotCreator/commit/3e792a8b6a2271c9f3031a1fd2fa8538baa516f6))
* **deps:** update mcr.microsoft.com/dotnet/sdk docker tag to v10.0.402 ([31e24a3](https://www.github.com/mu88/ScreenshotCreator/commit/31e24a344bdeb5e39a66c1d8b5681ddb126ef7dc))
* **deps:** update mu88.Shared to version 7.0.0 for latest OTEL fixes ([13d39ed](https://www.github.com/mu88/ScreenshotCreator/commit/13d39ed1fbb8f968856ae5b20028fcffe525451e))
* **deps:** update mu88/common digest to 03ce929 ([2c4c8f3](https://www.github.com/mu88/ScreenshotCreator/commit/2c4c8f33aab1a3d90b55c55267f55d79daa45258))
* **deps:** update mu88/common digest to 0d04e69 ([4891ce1](https://www.github.com/mu88/ScreenshotCreator/commit/4891ce1810cc63a89a8b8c3fe0432185488e3de2))
* **deps:** update mu88/common digest to 2edcdac ([4619c5d](https://www.github.com/mu88/ScreenshotCreator/commit/4619c5d49711934e536dde6683aa470480c50f92))
* **deps:** update mu88/common digest to 309b9cb ([2f10eeb](https://www.github.com/mu88/ScreenshotCreator/commit/2f10eebd605aeca625908cc1477f0fd75d3efc84))
* **deps:** update mu88/common digest to 349db23 ([213d78c](https://www.github.com/mu88/ScreenshotCreator/commit/213d78c890ef92250ffc8defcc4ff5f384c28ae7))
* **deps:** update mu88/common digest to 5d4855b ([dbe3601](https://www.github.com/mu88/ScreenshotCreator/commit/dbe3601090ccf087c0d28370c94d6727666e26a9))
* **deps:** update mu88/common digest to 64260e9 ([5c4173c](https://www.github.com/mu88/ScreenshotCreator/commit/5c4173c83d62284f69c09e87def7a175050b9e1d))
* **deps:** update mu88/common digest to 9a7d93f ([96c98cd](https://www.github.com/mu88/ScreenshotCreator/commit/96c98cd2e39bb416027bf4208b690cec9a9481d9))
* **deps:** update mu88/common digest to bf4949c ([6a014a0](https://www.github.com/mu88/ScreenshotCreator/commit/6a014a064cac42a6ace810c5d59b758f6f9b10c0))
* **deps:** update mu88/common digest to e3c88ef ([fc65df7](https://www.github.com/mu88/ScreenshotCreator/commit/fc65df78f1bfc2265f02718b78724b9a71a6bc31))
* **deps:** update playwright to 1.60.0 ([3a21841](https://www.github.com/mu88/ScreenshotCreator/commit/3a21841559a4322411af03bb0a24da95461abba3))
* **deps:** upgrade Magick.NET to resolve security vulnerability ([1dbcbc1](https://www.github.com/mu88/ScreenshotCreator/commit/1dbcbc180981c52c6f9448cbd17c7f420083217e))
* **deps:** upgrade mu88.Shared for latest OTel / health check updates ([23c7c0d](https://www.github.com/mu88/ScreenshotCreator/commit/23c7c0de9985edfaafe600231f19fe7e6f974f95))
* **deps:** upgrade mu88.Shared to version 5.2.0 for improved OTEL functionality ([97a11b0](https://www.github.com/mu88/ScreenshotCreator/commit/97a11b077fbb56f9d94b180736ff65fccfc27cdd))
* **deps:** upgrade mu88.Shared to version 6.0.0 for latest OTEL fixes ([75dd3fb](https://www.github.com/mu88/ScreenshotCreator/commit/75dd3fb098b8bb34436efb018400a05a7d0b2561))
* **deps:** upgrade openHAB to v5.1.1 ([7a87a1a](https://www.github.com/mu88/ScreenshotCreator/commit/7a87a1a37d3d86e06cf42488d979edba685b2cc8))
* **dev:** fix broken DotSettings links ([577cc77](https://www.github.com/mu88/ScreenshotCreator/commit/577cc77d2d234e5ca47f2fd361aba809d3b6bd50))
* **renovate:** remove redundant mcr.microsoft.com timestamp exceptions ([de09b19](https://www.github.com/mu88/ScreenshotCreator/commit/de09b198fbae7733ba6b515812d1b8dea129ef16))

### Breaking Changes

* migrate application to .NET 11 ([f247f89](https://www.github.com/mu88/ScreenshotCreator/commit/f247f894dd884058596b00c60f31d50f91ad7a15))

<a name="5.0.0"></a>
## [5.0.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v5.0.0) (2025-12-12)

### ✨ Features

* upgrade to .NET 10 ([d191c56](https://www.github.com/mu88/ScreenshotCreator/commit/d191c5637f5d7a3f6502fdaccc717dd3d3b8a238))

### ♻️ Refactors

* replace Swashbuckle with Microsoft's OpenAPI package and use Scalar for UI ([07e8554](https://www.github.com/mu88/ScreenshotCreator/commit/07e8554ac19ac6f87abe2a900030be7107ec4b13))

### ✅ Tests

* fix system test ([30494ae](https://www.github.com/mu88/ScreenshotCreator/commit/30494ae7c5be6e36ac8ec2fb48ad053ad9ef78c4))

### 🔧 Chores

* switch to new SLNX format ([97c9c8e](https://www.github.com/mu88/ScreenshotCreator/commit/97c9c8ec2e81ab8ee6a490c52a57d6007bead847))
* **deps:** patch Magick due to security vulnerability ([19ff6c0](https://www.github.com/mu88/ScreenshotCreator/commit/19ff6c054651341cf25056955c90c9db36afa98c))
* **deps:** update dependency microsoft.playwright to 1.56.0 ([f29c2aa](https://www.github.com/mu88/ScreenshotCreator/commit/f29c2aaf7a2678ed50f83898b7b2848941e849cd))

### Breaking Changes

* upgrade to .NET 10 ([d191c56](https://www.github.com/mu88/ScreenshotCreator/commit/d191c5637f5d7a3f6502fdaccc717dd3d3b8a238))

<a name="4.0.0"></a>
## [4.0.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v4.0.0) (2025-10-05)

### Bug Fixes

* **deps:** use correct .NET SDK version within dev container ([f65e153](https://www.github.com/mu88/ScreenshotCreator/commit/f65e153da65a03c4080a743f3e8ea31146a495ef))

### Breaking Changes

* use GitHub Container Registry instead of Docker Hub ([59ad9d8](https://www.github.com/mu88/ScreenshotCreator/commit/59ad9d8d8fae5618a5de078e56a0a95727ecdbe6))

<a name="3.1.0"></a>
## [3.1.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v3.1.0) (2024-12-20)

### Features

* add health check ([ef587a7](https://www.github.com/mu88/ScreenshotCreator/commit/ef587a77d61fe6313ed674ffdc463f6bd9bbf978))
* add reusable workflow ([4a1d6b3](https://www.github.com/mu88/ScreenshotCreator/commit/4a1d6b35be222619618fc61ce77d11a9c3be54d1))
* embed health check tool ([e5f8fec](https://www.github.com/mu88/ScreenshotCreator/commit/e5f8fec6d5bd7d78a3b56830d75490e827e43c19))

<a name="3.0.0"></a>
## [3.0.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v3.0.0) (2024-12-06)

### Features

* **deps:** upgrade to .NET 9 ([f240edc](https://www.github.com/mu88/ScreenshotCreator/commit/f240edcd3c39bf46a9c945085836b9b2aebfe5df))

### Breaking Changes

* **deps:** upgrade to .NET 9 ([f240edc](https://www.github.com/mu88/ScreenshotCreator/commit/f240edcd3c39bf46a9c945085836b9b2aebfe5df))

<a name="2.8.0"></a>
## [2.8.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.8.0) (2024-10-05)

### Features

* remove config option "BackgroundProcessingWithTryCatch" ([3c4252f](https://www.github.com/mu88/ScreenshotCreator/commit/3c4252fec193b1223b7276ba6c41549031856bd2))

<a name="2.7.1"></a>
## [2.7.1](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.7.1) (2024-08-03)

<a name="2.7.0"></a>
## [2.7.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.7.0) (2024-08-02)

### Features

* use MSBuild targets for building multi-arch Docker images from NuGet package mu88.Shared ([b352582](https://www.github.com/mu88/ScreenshotCreator/commit/b352582d698bb11f1a668a53391311b133866aa1))

<a name="2.6.2"></a>
## [2.6.2](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.6.2) (2024-08-02)

### Bug Fixes

* readd multi-arch Docker target again ([5743d93](https://www.github.com/mu88/ScreenshotCreator/commit/5743d936e02b0dae3c7fc08b96a1609d40a0a3a1))

<a name="2.6.1"></a>
## [2.6.1](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.6.1) (2024-08-02)

<a name="2.6.0"></a>
## [2.6.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.6.0) (2024-08-02)

### Features

* replace OpenTelemetry and multi-manifest Docker image logic with NuGet package mu88.Shared ([4be013c](https://www.github.com/mu88/ScreenshotCreator/commit/4be013cd041236e0c419de3f64e1acb476465325))

<a name="2.5.2"></a>
## [2.5.2](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.5.2) (2024-07-17)

### Bug Fixes

* do not reuse Playwright session as this leaks memory ([0b4592f](https://www.github.com/mu88/ScreenshotCreator/commit/0b4592f3d14d1d346c65dfc27abbb77b69c19eba))

<a name="2.5.1"></a>
## [2.5.1](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.5.1) (2024-07-15)

<a name="2.5.0"></a>
## [2.5.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.5.0) (2024-07-10)

### Features

* add options to configure page reuse and try-catch ([df65611](https://www.github.com/mu88/ScreenshotCreator/commit/df65611b39c0d6969383b55dd323ff422afae753))

<a name="2.4.0"></a>
## [2.4.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.4.0) (2024-07-07)

### Features

* keep background service running when exception occurs ([f2f9cef](https://www.github.com/mu88/ScreenshotCreator/commit/f2f9cefcb2051a757506aaf75be661deb1d029b2))

<a name="2.3.1"></a>
## [2.3.1](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.3.1) (2024-06-29)

<a name="2.3.0"></a>
## [2.3.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.3.0) (2024-06-16)

### Features

* add OpenTelemetry ([4255ad3](https://www.github.com/mu88/ScreenshotCreator/commit/4255ad3719234f33b77d8bef008678aba9662703))

<a name="2.2.3"></a>
## [2.2.3](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.2.3) (2024-02-09)

<a name="2.2.2"></a>
## [2.2.2](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.2.2) (2024-02-09)

<a name="2.2.1"></a>
## [2.2.1](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.2.1) (2024-02-09)

<a name="2.2.0"></a>
## [2.2.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.2.0) (2023-11-19)

### Features

* detect (hidden) login button before logging in ([dac768c](https://www.github.com/mu88/ScreenshotCreator/commit/dac768ca6b3fcfd6f1a630eb97b855dcc8ac24bf))

<a name="2.1.1"></a>
## [2.1.1](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.1.1) (2023-11-19)

<a name="2.1.0"></a>
## [2.1.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.1.0) (2023-11-18)

### Features

* introduce concept of availability indicator ([1f78793](https://www.github.com/mu88/ScreenshotCreator/commit/1f787938034ff155c184b567c9e4395d84b8a3d5))

<a name="2.0.1"></a>
## [2.0.1](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.0.1) (2023-11-17)

### Bug Fixes

* assign proper permissions to create file ([5533728](https://www.github.com/mu88/ScreenshotCreator/commit/5533728787a366d93000e481e8545d86b3915e14))

<a name="2.0.0"></a>
## [2.0.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v2.0.0) (2023-11-17)

### Features

* use .NET 8 ([a62666d](https://www.github.com/mu88/ScreenshotCreator/commit/a62666d7ea2d076e42535fe34debb41cda9f520c))

### Breaking Changes

* use .NET 8 ([a62666d](https://www.github.com/mu88/ScreenshotCreator/commit/a62666d7ea2d076e42535fe34debb41cda9f520c))

<a name="1.21.0"></a>
## [1.21.0](https://www.github.com/mu88/ScreenshotCreator/releases/tag/v1.21.0) (2023-11-17)

### Features

* add Docker support ([08bea34](https://www.github.com/mu88/ScreenshotCreator/commit/08bea3403e7a8b61281c379c8d21bd13b9140c67))
* add image creation time ([8543d7c](https://www.github.com/mu88/ScreenshotCreator/commit/8543d7cb1aa187c31523938db52abfeb28c9aaf6))
* add Waveshare instructions (activity) ([010c7a9](https://www.github.com/mu88/ScreenshotCreator/commit/010c7a960d6d104894c1e24dc6c5f3f0c907a058))
* always create image when starting and using background processor ([76c54ae](https://www.github.com/mu88/ScreenshotCreator/commit/76c54ae82489bb8b7f8b4c310102f50f78b296cb))
* apply activity to background service ([e60e491](https://www.github.com/mu88/ScreenshotCreator/commit/e60e4918418145e85c31aa2eac434d571e18f8cc))
* avoid unnecessary call to dashboard URL ([04a84aa](https://www.github.com/mu88/ScreenshotCreator/commit/04a84aa515e57235f7914bf305a8ec996d58bbdc))
* consume less power and extract sensitive data into config file ([c374d89](https://www.github.com/mu88/ScreenshotCreator/commit/c374d89dbd3ec8ff666bc3b113ac5daa18cfc2c5))
* create new endpoint for image creation by width and height ([a6d8406](https://www.github.com/mu88/ScreenshotCreator/commit/a6d84067e078655bbd3c10d176a6a443c44062ba))
* display current battery status on Waveshare display ([e71694c](https://www.github.com/mu88/ScreenshotCreator/commit/e71694c66a99247b5f3bdc3b8c54e46cf4c5b237))
* host under sub-resource ([522a313](https://www.github.com/mu88/ScreenshotCreator/commit/522a313c991bd48cb1992f1bd5f401d660dfb154))
* implement background screenshot creator ([4112ec4](https://www.github.com/mu88/ScreenshotCreator/commit/4112ec4ea97fc6e625a1a5a7f7b288068f55434f))
* make background processing optional ([3afd358](https://www.github.com/mu88/ScreenshotCreator/commit/3afd358dc6ee9eab33dce91efd1969063cbcc796))
* provide image as black/white and bytes-only ([9916aa0](https://www.github.com/mu88/ScreenshotCreator/commit/9916aa0eca59340ae7757e9d46de3c08b238fb44))
* reuse Playwright page ([83ca7be](https://www.github.com/mu88/ScreenshotCreator/commit/83ca7bedee3ce57a6973dbc10a6c63d12b886066))
* set last modification time as custom HTTP header ([f4e2708](https://www.github.com/mu88/ScreenshotCreator/commit/f4e2708ec4bc2f11dbf34579cb23c8ff76f615b9))
* use Raspi's deep sleep ([c27b698](https://www.github.com/mu88/ScreenshotCreator/commit/c27b698ed786b0d4a84a1b2df5e1db6a321be444))

### Bug Fixes

* change order of columns and rows ([760598f](https://www.github.com/mu88/ScreenshotCreator/commit/760598fd8799b07ef6d348e9a8982830dfb9fca6))
* check for other login string ([5c88805](https://www.github.com/mu88/ScreenshotCreator/commit/5c888055d3d4486237fd04e115beb872ae9a122c))
* don't use sleep APIs from machine module ([4d09f25](https://www.github.com/mu88/ScreenshotCreator/commit/4d09f255dabee39cf638809368dfc551a5384f7b))
* process pixels correctly by inverting bits ([c17048a](https://www.github.com/mu88/ScreenshotCreator/commit/c17048a8acfe2910598efc9a0704fffbe3f3ae83))
* rename config parameter in settings ([7c68074](https://www.github.com/mu88/ScreenshotCreator/commit/7c68074dd512131ec51744df92fb2b32a6a32b5d))
* use fixed version 3.4.4 of openHAB until Playwright problem is solved ([76eaa06](https://www.github.com/mu88/ScreenshotCreator/commit/76eaa06daee02bc81be3b3bfa90ad038b2009888))

