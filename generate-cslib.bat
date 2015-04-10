@echo off
packages\AutoRest.0.9.7\tools\AutoRest.exe -CodeGenerator CSharp -Modeler Swagger -Input Spec\oanda-rest.json -Namespace Oanda.Rest -OutputDirectory SmartQuant.Oanda\Oanda.Rest.Generated
@echo on