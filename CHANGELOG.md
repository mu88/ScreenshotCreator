# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

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

