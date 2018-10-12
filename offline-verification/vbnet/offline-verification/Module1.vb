Imports SKM.V3
Imports SKM.V3.Methods
Imports SKM.V3.Models

Module Module1

    Sub Main()
        Dim licenseKey = "JJWTE-MVKLR-YQMKD-IFXUV"
        Dim RSAPubKey = "<RSAKeyValue><Modulus>wflqVfb+6gkb1lHr1fysux8o+oJREy0M2hM3yEKh92w5rXoc673ZRI9JTg725IqHvr/031TMqFAMvfdQ8X5gB0L3gb3E1tY/QvpZxwobRs6Xpz3XxuGhZ7cIhO9uWLZuykSvoD+PQZMOsdp+M05p9KS4eTsbuSo1w0kwkp6AnumMiG7ICOxYsdGg7YlzX5DN3m6QgG9Fg+6faIOLyBxXktBK4+ligCMNYdRLd3z9UqhGGy4u/Hnz3VD2bzJ99JWQYx00HtXyojmUHi25Yk2G5eki3sQXM7gYzWkZOzaEQ5/rMghp0O3/eJblWtranxQv4HBCm/b8l4oVh4GTcN0x7w==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"

        Dim auth = "WyIxMDQwIiwiK094dW9yUWxkMitmYTBIaTVQdDBsYXpKenpoUithK2IyQVR3KzlzZCJd"

        Dim result = Key.Activate(token:=auth, parameters:=New ActivateModel() With {
                                  .Key = licenseKey,
                                  .ProductId = 3843,
                                  .Sign = True,
                                  .MachineCode = Helpers.GetMachineCode()
                                  })

        If result Is Nothing OrElse result.Result = ResultType.[Error] OrElse
            Not result.LicenseKey.HasValidSignature(RSAPubKey).IsValid Then
            ' an error occurred or the key is invalid or it cannot be activated
            ' (eg. the limit of activated devices was achieved)

            ' -------------------New code starts -------------------
            ' we will try to check for a cached response/certificate

            Dim licensefile As New LicenseKey()

            If licensefile.LoadFromFile("licensefile").HasValidSignature(RSAPubKey, 3).IsValid() Then
                Console.WriteLine("The license is valid!")
            Else
                Console.WriteLine("The license does not work.")
            End If
            ' -------------------new code ends ---------------------
        Else
            ' everything went fine if we are here!
            Console.WriteLine("The license is valid!")

            ' -------------------New code starts -------------------
            ' saving a copy of the response/certificate
            result.LicenseKey.SaveToFile("licensefile")
            ' -------------------New code ends ---------------------
        End If


        Console.ReadLine()
    End Sub




End Module
