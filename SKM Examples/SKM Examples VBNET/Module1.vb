Imports SKGL
Module Module1

    Sub Main()
        KeyActivation()
        AddFeature()
        RemoveFeature()

        Console.ReadLine()


    End Sub


    Private Sub KeyActivation()

        Dim machineCode As String = SKM.getMachineCode(AddressOf SKM.getSHA1)
        ' using SHA1 as the hashing algorithm
        Dim licensekey As String = "MNIVR-MGQRL-QGUZK-BGJHQ"

        Dim keyinfo = SKM.KeyActivation(pid:="3", uid:="2", hsum:="751963", sid:=licensekey, mid:=machineCode)

        Dim newKey = keyinfo.NewKey

        If keyinfo.IsValid() Then
            'valid key

            Console.WriteLine("Created:" & keyinfo.CreationDate.ToShortDateString())

            Console.WriteLine("Expired?: " & keyinfo.HasNotExpired().IsValid())
        Else
            'invalid key
            Console.WriteLine("Failed activation.")
        End If

    End Sub


    Private Sub AddFeature()
        Dim keydata = New FeatureModel() With {
            .Key = "LXWVI-HSJDU-CADTC-BAJGW",
            .Feature = 2,
            .ProductId = 3349
        }

        Dim auth = New AuthDetails() With {
            .Token = "WyI2Iiwib3lFQjFGYk5pTHYrelhIK2pveWdReDdEMXd4ZDlQUFB3aGpCdTRxZiJd"
        }


        Dim result = SKM.AddFeature(auth, keydata)

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

        Dim auth = New AuthDetails() With {
            .Token = "WyI2Iiwib3lFQjFGYk5pTHYrelhIK2pveWdReDdEMXd4ZDlQUFB3aGpCdTRxZiJd"
        }

        Dim result = SKM.RemoveFeature(auth, keydata)

        If result IsNot Nothing AndAlso result.Result = ResultType.Success Then
            ' feature 2 is set to false.
            Console.WriteLine("Feature 2 set to false")
        Else
            Console.WriteLine("Feature 2 was not changed because of an error: " + result.Message)
        End If

    End Sub

End Module
