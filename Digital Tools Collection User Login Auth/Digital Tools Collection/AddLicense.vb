Imports SKM.V3
Imports SKM.V3.Models
Imports SKM.V3.Methods

Public Class AddLicense



    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Dim token = "WyIxNzIiLCJhak9OT1g3NW90YlQyRFFVUzBWdnlGSHJYdUpMdDA0REMxNzNOa2duIl0="
        Dim key = TextBox1.Text.Replace(" ", "")

        Dim license = New LicenseKey() With
        {
            .ProductId = 3349,
            .Key = key
        }

        If license.Refresh(token, True) Then
            ' we are able to auto complete missing key info

            Form1.Button1.Enabled = license.HasFeature(3).IsValid() ' either we have feature1 or not.
            Form1.Button2.Enabled = license.HasFeature(4).IsValid() ' either we have feature2 or not.
            Form1.Button4.Enabled = license.HasFeature(5).IsValid() ' either we have feature3 or not.

            Form1.Text = "Digital Tools"

            If license.HasFeature(1).HasNotExpired().IsValid() Then
                ' feature 1 is a time limited, so we check that it has not expired.
                Form1.Text = "Digital Tools - " + license.DaysLeft().ToString() + " day(s) left"
            ElseIf license.HasNotFeature(1).IsValid() Then



            Else
                MsgBox("Your license has expired and cannot be used.")
                nolicense()

            End If

            license.SaveToFile()

        Else
            ' something went wrong.
            MsgBox("Unable to access the license server or the key is wrong.")

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