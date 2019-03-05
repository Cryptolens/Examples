import io.cryptolens.internal.*;
import io.cryptolens.methods.*;
import io.cryptolens.models.*;

public class Main {

    public static void main(String[] args) {

        // note, if we ran Key Verification earlier, we can can set Key=license.KeyString

        String auth = "WyIyOTA3IiwiMnNwUTFnb1JNQUt5U204UzR4ZzVkRUpQaWV6cEVXbCtrSkRod2pRTCJd";

        ListOfDataObjectsResult listResult = Data.ListDataObjects(auth, new ListDataObjectsToKeyModel(3941, "FRQHQ-FSOSD-BWOPU-KJOWF", "usagecount"));

        if (!Helpers.IsSuccessful(listResult)) {
            System.out.println("Could not list the data objects.");
        }

        if (listResult.DataObjects == null) {
            BasicResult addResult = Data.AddDataObject(auth,
                    new AddDataObjectToKeyModel(
                            3941,
                            "FRQHQ-FSOSD-BWOPU-KJOWF",
                            "usagecount",
                            0,
                            ""));

            if (!Helpers.IsSuccessful(addResult)) {
                System.out.println("Could not add a new data object. Maybe the limit was reached?");
            }
        } else {
            // if you set enableBound=true and bound=50 (as an example)
            // it won't be possible to increase to a value greater than 50.
            BasicResult incrementResult = Data.IncrementIntValue(auth,
                    new IncrementIntValueToKeyModel(
                            3941,
                            "FRQHQ-FSOSD-BWOPU-KJOWF",
                            listResult.DataObjects.get(0).Id,
                            1,
                            false,
                            0));

            if(!Helpers.IsSuccessful(incrementResult)) {
                System.out.println("Could not increment the data object");
            }
        }
    }
}
