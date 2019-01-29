Imports SKM.V3
Imports SKM.V3.Models
Imports SKM.V3.Methods

Public Class AddLicense



    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        ' The code below is based on https://help.cryptolens.io/examples/key-verification.

        Dim token = "WyIyNjAxIiwiL0ZYYndDTm1jTlJNdGRPeDFNS29iNnlaQSs1NTVkZHcyREVWZXFDdyJd"
        Dim RSAPubKey = "<RSAKeyValue><Modulus>sGbvxwdlDbqFXOMlVUnAF5ew0t0WpPW7rFpI5jHQOFkht/326dvh7t74RYeMpjy357NljouhpTLA3a6idnn4j6c3jmPWBkjZndGsPL4Bqm+fwE48nKpGPjkj4q/yzT4tHXBTyvaBjA8bVoCTnu+LiC4XEaLZRThGzIn5KQXKCigg6tQRy0GXE13XYFVz/x1mjFbT9/7dS8p85n8BuwlY5JvuBIQkKhuCNFfrUxBWyu87CFnXWjIupCD2VO/GbxaCvzrRjLZjAngLCMtZbYBALksqGPgTUN7ZM24XbPWyLtKPaXF2i4XRR9u6eTj5BfnLbKAU5PIVfjIS+vNYYogteQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
        Dim keyStr = TextBox1.Text.Replace(" ", "")

        Dim result = Key.Activate(token:=token, parameters:=New ActivateModel() With {
                          .Key = keyStr,
                          .ProductId = 3349,
                          .Sign = True,
                          .MachineCode = Helpers.GetMachineCode()
                          })

        If result Is Nothing OrElse result.Result = ResultType.[Error] OrElse
            Not result.LicenseKey.HasValidSignature(RSAPubKey).IsValid Then
            ' an error occurred or the key is invalid or it cannot be activated
            ' (eg. the limit of activated devices was achieved)
            MsgBox("Unable to access the license server or the key is wrong.")

        Else
            ' everything went fine if we are here!

            Dim license = result.LicenseKey

            Form1.Button1.Enabled = license.HasFeature(1).IsValid() ' either we have feature1 or not.
            Form1.Button2.Enabled = license.HasFeature(2).IsValid() ' either we have feature2 or not.
            Form1.Button4.Enabled = license.HasFeature(3).IsValid() ' either we have feature3 or not.

            Form1.Text = "Digital Tools"

            If license.HasFeature(4).HasNotExpired().IsValid() Then
                ' feature 1 is a time limited, so we check that it has not expired.
                Form1.Text = "Digital Tools - " + license.DaysLeft().ToString() + " day(s) left"
            ElseIf license.HasNotFeature(4).IsValid() Then

            Else
                MsgBox("Your license has expired and cannot be used.")
                nolicense()

            End If

            license.SaveToFile()

        End If

        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Sub nolicense()
        Form1.Button1.Enabled = False
        Form1.Button2.Enabled = False
        Form1.Button4.Enabled = False
    End Sub
End Class