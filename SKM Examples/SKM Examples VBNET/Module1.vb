Imports SKM.V3
Imports SKM.V3.Models
Imports SKM.V3.Methods
Module Module1

    Sub Main()
        'KeyActivation()
        'AddFeature()
        'RemoveFeature()
        GetKeysExample()
        Console.ReadLine()
    End Sub

    Private Sub GetKeysExample()
        Dim parameters = New GetKeysModel() With {
            .ProductId = 3,
            .Page = 1
        }

        Dim auth = "{access token with GetKeys permission and optional product lock}"

        Dim result = Product.GetKeys(token:=auth, parameters:=parameters)

        If (result IsNot Nothing AndAlso result.Result = ResultType.Success) Then
            ' successful 

            ' displays the first 99 keys of the product.
            ' simply increment Page to 2 in order to get
            ' the rest.
            For Each key As LicenseKey In result.LicenseKeys
                Console.WriteLine(key.Key)
            Next

        End If

    End Sub

    Private Sub GenerateKey()

        Dim parameters = New CreateKeyModel() With {
            .ProductId = 3,
            .F1 = 1,
            .Period = 30
        }

        Dim auth = "{access token with CreateKey permission and optional product lock}"

        Dim result = Key.CreateKey(token:=auth, parameters:=parameters)

        If (result IsNot Nothing AndAlso result.Result = ResultType.Success) Then
            ' successful
            Console.WriteLine(result.Key)
        End If


    End Sub



    Private Sub AddFeature()
        Dim keydata = New FeatureModel() With {
            .Key = "LXWVI-HSJDU-CADTC-BAJGW",
            .Feature = 2,
            .ProductId = 3349
        }

        Dim auth = "WyI2Iiwib3lFQjFGYk5pTHYrelhIK2pveWdReDdEMXd4ZDlQUFB3aGpCdTRxZiJd"


        Dim result = Key.AddFeature(auth, keydata)

        If result IsNot Nothing AndAlso result.Result = ResultType.Success Then
            ' feature 2 is set to true.
            Console.WriteLine("feature 2 set to true")
        Else
            Console.WriteLine("feature 2 was not changed because of an error: " + result.Message)
        End If

    End Sub



    Private Sub RemoveFeature()

        Dim keydata = New FeatureModel() With {
            .Key = "LXWVI-HSJDU-CADTC-BAJGW",
            .Feature = 2,
            .ProductId = 3349
        }

        Dim auth = "WyI2Iiwib3lFQjFGYk5pTHYrelhIK2pveWdReDdEMXd4ZDlQUFB3aGpCdTRxZiJd"


        Dim result = key.RemoveFeature(auth, keydata)

        If result IsNot Nothing AndAlso result.Result = ResultType.Success Then
            ' feature 2 is set to false.
            Console.WriteLine("Feature 2 set to false")
        Else
            Console.WriteLine("Feature 2 was not changed because of an error: " + result.Message)
        End If

    End Sub

End Module
