Imports SKM.V3

Public Class Form1
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        NoLicense()

        Dim license = New LicenseKey().LoadFromFile()
        Dim publicKey = "<RSAKeyValue><Modulus>sGbvxwdlDbqFXOMlVUnAF5ew0t0WpPW7rFpI5jHQOFkht/326dvh7t74RYeMpjy357NljouhpTLA3a6idnn4j6c3jmPWBkjZndGsPL4Bqm+fwE48nKpGPjkj4q/yzT4tHXBTyvaBjA8bVoCTnu+LiC4XEaLZRThGzIn5KQXKCigg6tQRy0GXE13XYFVz/x1mjFbT9/7dS8p85n8BuwlY5JvuBIQkKhuCNFfrUxBWyu87CFnXWjIupCD2VO/GbxaCvzrRjLZjAngLCMtZbYBALksqGPgTUN7ZM24XbPWyLtKPaXF2i4XRR9u6eTj5BfnLbKAU5PIVfjIS+vNYYogteQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"

        Dim token = "WyIxNzIiLCJhak9OT1g3NW90YlQyRFFVUzBWdnlGSHJYdUpMdDA0REMxNzNOa2duIl0="

        If license IsNot Nothing Then

            ' either we get a fresh copy of the license or we use the existing one (given it is no more than 90 days old)
            If license.Refresh(token, True) Or license.HasValidSignature(publicKey, 90).IsValid() Then

                Button1.Enabled = license.HasFeature(3).IsValid() ' either we have feature1 or not.
                Button2.Enabled = license.HasFeature(4).IsValid() ' either we have feature2 or not.
                Button4.Enabled = license.HasFeature(5).IsValid() ' either we have feature3 or not.

                Text = "Digital Tools"

                If license.HasFeature(1).HasNotExpired().IsValid() Then
                    ' feature 1 is a time limited, so we check that it has not expired.
                    Text = "Digital Tools - " + license.DaysLeft().ToString() + " day(s) left"
                ElseIf license.HasNotFeature(1).IsValid() Then
                    ' not time limited.

                Else
                    MsgBox("Your license has expired and cannot be used.")
                    NoLicense()

                End If

                license.SaveToFile()

            Else
                MsgBox("Your license has expired and cannot be used.")
            End If

        Else
            ' no license found. you could tell the user to provide a license key.
        End If


    End Sub

    Public Sub NoLicense()
        Button1.Enabled = False
        Button2.Enabled = False
        Button4.Enabled = False
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        AddLicense.ShowDialog()

    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        Try
            Process.Start("http://skmapp.com")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Buttons_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click, Button2.Click, Button4.Click
        MsgBox("It works, Enjoy! :)")
    End Sub

End Class
