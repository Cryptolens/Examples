Imports SKGL
Module Module1

    Sub Main()
        KeyActivation()

    End Sub


    Private Sub KeyActivation()

        Dim machineCode As String = SKM.getMachineCode(AddressOf SKM.getSHA1)
        ' using SHA1 as the hashing algorithm
        Dim licensekey As String = "MNIVR-MGQRL-QGUZK-BGJHQ"

        Dim keyinfo = SKM.KeyActivation(pid:="3", uid:="2", hsum:="751963", sid:=licensekey, mid:=machineCode)

        Dim newKey = keyinfo.NewKey

        If keyinfo.IsValid() Then
            'valid key

            Console.WriteLine("Created:" + keyinfo.CreationDate.ToShortDateString())

            Console.WriteLine("Expired?: " + keyinfo.HasNotExpired().IsValid())
        Else
            'invalid key
            Console.WriteLine("Failed activation.")
        End If

    End Sub
End Module
