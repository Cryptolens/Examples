Imports System.Security.Cryptography
Imports Cryptolens.SKM.Auth
Imports Newtonsoft.Json
Imports SKM.V3
Imports SKM.V3.Models

Public Class Form1
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        NoLicense()

        RefreshLicense()


    End Sub

    Public Sub NoLicense()
        Button1.Enabled = False
        Button2.Enabled = False
        Button4.Enabled = False
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click

        RefreshLicense()

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


    Private Sub RefreshLicense()
        If My.Settings.usertoken = "" Then
            My.Settings.usertoken = Nothing
        End If


        ' This is found at https://serialkeymanager.com/User/Security
        Dim RSAPublicKey = New RSACryptoServiceProvider(2048)
        RSAPublicKey.FromXmlString("<RSAKeyValue><Modulus>sGbvxwdlDbqFXOMlVUnAF5ew0t0WpPW7rFpI5jHQOFkht/326dvh7t74RYeMpjy357NljouhpTLA3a6idnn4j6c3jmPWBkjZndGsPL4Bqm+fwE48nKpGPjkj4q/yzT4tHXBTyvaBjA8bVoCTnu+LiC4XEaLZRThGzIn5KQXKCigg6tQRy0GXE13XYFVz/x1mjFbT9/7dS8p85n8BuwlY5JvuBIQkKhuCNFfrUxBWyu87CFnXWjIupCD2VO/GbxaCvzrRjLZjAngLCMtZbYBALksqGPgTUN7ZM24XbPWyLtKPaXF2i4XRR9u6eTj5BfnLbKAU5PIVfjIS+vNYYogteQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>")

        Dim authRequest = UserLoginAuth.GetLicenseKeys(SKGL.SKM.getMachineCode(AddressOf SKGL.SKM.getSHA256), "WyI2NzMiLCJjbnRyeEUxMXdQMUt3VFg1L2F0bFFScnNtemxteGZqU0RoQXFCTWRDIl0=", "Test Application", 5, RSAPublicKey.ExportParameters(False), My.Settings.usertoken,
            New RSACryptoServiceProvider(2048))

        If authRequest.[error] Is Nothing Then
            Dim data = JsonConvert.DeserializeObject(Of GetLicenseKeysResult)(authRequest.jsonResult)

            Dim licenses = JsonConvert.DeserializeObject(Of List(Of KeyInfoResult))(System.Text.UTF8Encoding.UTF8.GetString(Convert.FromBase64String(data.Results)))

            Dim voiceRecorder = licenses.Where(Function(x) x.LicenseKey.ProductId = 3349 AndAlso x.LicenseKey.F1 = True AndAlso x.LicenseKey.HasNotExpired().IsValid())
            Dim videoRecorder = licenses.Where(Function(x) x.LicenseKey.ProductId = 3349 AndAlso x.LicenseKey.F2 = True AndAlso x.LicenseKey.HasNotExpired().IsValid())
            Dim converter = licenses.Where(Function(x) x.LicenseKey.ProductId = 3349 AndAlso x.LicenseKey.F3 = True AndAlso x.LicenseKey.HasNotExpired().IsValid())

            If voiceRecorder.Count() >= 1 Then
                Button1.Enabled = True
            Else
                Button1.Enabled = False
            End If

            If videoRecorder.Count() >= 1 Then
                Button2.Enabled = True
            Else
                Button2.Enabled = False
            End If

            If converter.Count() >= 1 Then
                Button4.Enabled = True
            Else
                Button4.Enabled = False
            End If

            My.Settings.usertoken = authRequest.licenseKeyToken

        Else
            MsgBox("An error occurred or no valid license could be found.")
            My.Settings.usertoken = ""
        End If

    End Sub

End Class



Public Class GetLicenseKeysResult
    Inherits BasicResult
    Public Property Results() As String
        Get
            Return m_Results
        End Get
        Set
            m_Results = Value
        End Set
    End Property
    Private m_Results As String
    Public Property ActivatedMachineCodes() As String
        Get
            Return m_ActivatedMachineCodes
        End Get
        Set
            m_ActivatedMachineCodes = Value
        End Set
    End Property
    Private m_ActivatedMachineCodes As String
    Public Property Signature() As String
        Get
            Return m_Signature
        End Get
        Set
            m_Signature = Value
        End Set
    End Property
    Private m_Signature As String
End Class