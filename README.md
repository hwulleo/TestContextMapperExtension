# TestContextMapperExtension

An extension method to the NUnit TestContext for mapping config objects out of the runsettings. It is intended to be used as a way to pull [BrowserTypeLaunchOptions (from the Playwright web testing library)](https://playwright.dev/dotnet/docs/api/class-browsertype#browser-type-launch) directly from a .runsettings file. However, it is written so that other configuration objects should also work...as long as they are fairly simple.

This library is still very much **under construction**. 
-
