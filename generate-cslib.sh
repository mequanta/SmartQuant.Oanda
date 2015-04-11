#!/bin/bash

CMD=packages/AutoRest.0.9.7/tools/AutoRest.exe
mono ${CMD} -CodeGenerator CSharp -Modeler Swagger -Input Spec/oanda-rest.json -Namespace Oanda.Rest -ClientName Client -AddCredentials true -OutputFileName Client.Generated.cs -OutputDirectory SmartQuant.Oanda\Oanda.Rest

