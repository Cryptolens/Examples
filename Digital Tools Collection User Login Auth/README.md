# User Login Authentication
This examples shows how to authenticate your customers using a **user account** instead of a **license key**. Please note that this code is adjusted to .NET Framework 4.6.2. For other frameworks, please see [remarks](#remarks).

A video tutorial can be found [here](https://youtu.be/3GDwRUBgD4A).

## Idea
The main method that authenticates the user is shown in [Using the Code](#using-the-code) section. It's called at two time instances: when the program loads and when the user actively clicks on the "lock button" (i.e. log in button).

This code will behave as follows. First, it will notice that there is no saved access token so we won't do anything special. Once the user authorizes the request in the browser, we will store the returned `usertoken` as a settings variable. We will then verify that the user has the right features and that they have not expired (on a feature basis). Next time we would call SKM with he saved access token so that the user won't need to authenticate again. Once the `usertoken` expires or if there is a network error at a certain time, the `usertoken` will be reset and an error message shown. Next time the method is called (for instance when the user starts the application or clicks on the log in button), they will have to re-authenticate again.

## Using the Code
To make this code work in your solution, you need to change three parameters:

* **RSA Public Key** - it can be found at your [security page](https://serialkeymanager.com/User/Security).
* **Access Token** - an access token with _GetToken_ permission can be created [here](https://serialkeymanager.com/User/AccessToken#/newtoken).
* **The ProductId** - the product id is provided on the product page (you can select the desired product [here](https://serialkeymanager.com/Product))

> Please ensure that you have a Settings variable `usertoken`, to make `My.Settings.usertoken` work.

Before we include the main code, we need to add the following references:

```
Imports System.Security.Cryptography
Imports Cryptolens.SKM.Auth
Imports Newtonsoft.Json
Imports SKM.V3
Imports SKM.V3.Models
```

Note, you need to install both **SKGLExtension** and **Cryptolens.SKM**. More about this is found [here](https://github.com/SerialKeyManager/SKGL-Extension-for-dot-NET/blob/master/Tutorials/v.101-beta.md).

```
Private Sub RefreshLicense()

    If My.Settings.usertoken = "" Then
        My.Settings.usertoken = Nothing
    End If


    ' This is found at https://serialkeymanager.com/User/Security
    Dim RSAPublicKey = New RSACryptoServiceProvider(2048)
    RSAPublicKey.FromXmlString("RSA Public Key replace with yours")
    
    Dim authRequest = UserLoginAuth.GetLicenseKeys(SKGL.SKM.getMachineCode(AddressOf SKGL.SKM.getSHA256), "Access token with GetKey permission", "Test Application", 5, RSAPublicKey.ExportParameters(False), My.Settings.usertoken,
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

```

## Remarks

* For .NET Core applications, you don't need to add the last parameter, `RSACryptoServiceProvider(2048)`.
* Please see the [release notes](https://github.com/SerialKeyManager/SKGL-Extension-for-dot-NET/blob/master/Tutorials/v.101-beta.md) to get more info about creating new customers, including right packages, etc.
* It's very important that you add the right namespaces. The `GetLicenseKeysResult` is the same in both packages (i.e. in **SKGLExtension** and **Cryptolens.SKM**), so adding the wrong ones may give you errors.
