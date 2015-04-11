@echo off
packages\AutoRest.0.9.7\tools\AutoRest.exe -CodeGenerator CSharp -Modeler Swagger -Input Spec\oanda-rest.json -Namespace Oanda.Rest -ClientName Client -OutputFileName Client.Generated.cs -AddCredentials true -OutputDirectory SmartQuant.Oanda\Oanda.Rest
@echo on