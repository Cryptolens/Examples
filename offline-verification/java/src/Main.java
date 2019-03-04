import io.cryptolens.methods.*;
import io.cryptolens.models.*;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.PrintWriter;
import java.nio.file.Files;
import java.nio.file.Paths;

public class Main {

    public static void main(String[] args) {
        String RSAPubKey = "<RSAKeyValue><Modulus>sGbvxwdlDbqFXOMlVUnAF5ew0t0WpPW7rFpI5jHQOFkht/326dvh7t74RYeMpjy357NljouhpTLA3a6idnn4j6c3jmPWBkjZndGsPL4Bqm+fwE48nKpGPjkj4q/yzT4tHXBTyvaBjA8bVoCTnu+LiC4XEaLZRThGzIn5KQXKCigg6tQRy0GXE13XYFVz/x1mjFbT9/7dS8p85n8BuwlY5JvuBIQkKhuCNFfrUxBWyu87CFnXWjIupCD2VO/GbxaCvzrRjLZjAngLCMtZbYBALksqGPgTUN7ZM24XbPWyLtKPaXF2i4XRR9u6eTj5BfnLbKAU5PIVfjIS+vNYYogteQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        String auth = "WyIyODg0IiwianpVWC9aUUMzRGtXMFFkdUJ6NlVBcVdjWGxFdE1NbDFoaEFFZkxiSiJd";

        String currentMachineId = "test";

        LicenseKey license = Key.Activate(auth, RSAPubKey,
                new ActivateModel(3941,
                        "ETXYI-GFILQ-UXAXK-GGKSU-",
                        currentMachineId));

        if (license == null || !Helpers.IsOnRightMachine(license, currentMachineId)) {
            // an error occurred or the key is invalid or it cannot be activated
            // (eg. the limit of activated devices was achieved)

            // -------------------new code starts -------------------
            // we will try to check for a cached response/certificate

            String contents = "";
            try {
                contents = new String(Files.readAllBytes(Paths.get("licensefile.skm")));
            } catch (IOException e) {
                e.printStackTrace();
                return;
            }

            LicenseKey licenseFile = LicenseKey.LoadFromString(RSAPubKey, contents, 3);

            if(licenseFile != null && Helpers.IsOnRightMachine(license, currentMachineId)) {
                System.out.println("Offline mode");
                System.out.println("The license is valid!");
                System.out.println("It will expire: " + licenseFile.Expires);
            } else {
                System.out.println("The license does not work.");
            }

            // -------------------new code ends ---------------------

        } else {

            System.out.println("The license is valid!");
            System.out.println("It will expire: " + license.Expires);

            // -------------------new code starts -------------------
            // saving a copy of the response/certificate
            try {
                PrintWriter pw = new PrintWriter("licensefile.skm");
                pw.println(license.SaveAsString());
                pw.close();
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            }
            // -------------------new code ends ---------------------
        }
    }
}
