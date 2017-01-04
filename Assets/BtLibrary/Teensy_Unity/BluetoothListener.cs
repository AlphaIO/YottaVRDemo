using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TechTweaking.Bluetooth;
using UnityEngine.UI;

[System.Serializable]
public class BluetoothEvent : UnityEngine.Events.UnityEvent<string> { }

public class BluetoothListener : MonoBehaviour {
    public string macAddress;
    public string deviceName = "YOTTASENSE";
    public BluetoothEvent onDataReceived;
    private BluetoothDevice device;
    public FaceController[] FaceControllerArray;
    public Text debugText;
    void Awake() {
        BluetoothAdapter.askEnableBluetooth();//Ask user to enable Bluetooth
                                              //BluetoothAdapter.enableBluetooth(); //you can by this force enabling Bluetooth without asking the user
                                              //BluetoothAdapter.listenToBluetoothState() // if you want to listen to the following two events  OnBluetoothOFF or OnBluetoothON

        device = new BluetoothDevice();

        //In general pairing while Unity is working is troublesome. You better pair the device with your HC-06
        if (!string.IsNullOrEmpty(macAddress)) {
            device.MacAddress = "XX:XX:XX:XX:XX:XX";//It doesn't require paired devices, but it's advisable
        }
        else {
            device.Name = deviceName;//it only works with paired devices.
        }

        /*
		 * 10 equals the char '\n' which is a "new Line" in Ascci representation, 
		 * so the read() method will retun a packet that was ended by the byte 10. simply read() will read lines.
		 */
        device.setEndByte(10);
    }

    void Start() {
        BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound += HandleOnDeviceNotFound;

        connect();

   
    }

    void HandleOnDeviceOff(BluetoothDevice dev) {
        if (!string.IsNullOrEmpty(dev.Name))
            Debug.Log("Can't connect to " + dev.Name + ", device is OFF");
        else if (!string.IsNullOrEmpty(dev.MacAddress)) {
            Debug.Log("Can't connect to " + dev.MacAddress + ", device is OFF");
        }
    }
    void HandleOnDeviceNotFound(BluetoothDevice dev) {
        if (!string.IsNullOrEmpty(dev.Name))
            Debug.Log("Can't find " + dev.Name + ", device might be OFF or not paird yet ");
        else if (!string.IsNullOrEmpty(dev.MacAddress)) {
            Debug.Log("Can't find " + dev.MacAddress + ", device is OFF or not paired yet");
        }
    }

    //############### UI BUTTONS #####################
    public void connect()//Connect to the public global variable "device" if it's not null.
    {
        if (!BluetoothAdapter.isBluetoothEnabled()) {
            BluetoothAdapter.askEnableBluetooth();
            return;
        }

        if (device != null) {
            //please read about the different connect method. 
            //this normal_connect(..) method uses nothing fancy, just normal connection. 
            // the other method, connect(...) method,  will try different ways to connect and will take longer time
            // but it's mainly to support a wide variety of devices, and it has bigger chance of connection.
            device.connect();
            StartCoroutine(ManageConnection(device));
            //device.connect(false,false);
        }
    }

    public void disconnect() {
        if (device != null)
            device.close();
    }

    //############### Reading Data  #####################
    //Please note that you don't have to use Couroutienes, you can just put your code in the Update() method
    IEnumerator ManageConnection(BluetoothDevice device) {


        while (device.IsConnected && device.IsReading) {

            //polll all available packets
            BtPackets packets = device.readAllPackets();

            if (packets != null && packets.Count > 0) {

                /*
				 *  'packets' are ordered by indecies (0,1,2,3 ... N),
				 * where Nth packet is the latest packet and 0th is the oldest/first arrived packet.
				 * 
				 */

                /*
				* Ignore all old Texts within a single frame 
				*(for example the comming packets are ('angry', 'smily') where 'smily' is the last packet, the code will take 'smily')
				* "because it's a single frame" 
				*/

                int indx = packets.get_packet_offset_index(packets.Count - 1);
                int size = packets.get_packet_size(packets.Count - 1);

                string feeling = System.Text.ASCIIEncoding.ASCII.GetString(packets.Buffer, indx, size);
                Debug.Log("BLUETOOTH : " + device.Name + " Feels : " + feeling);
                debugText.text = " Feels : " + feeling;
                onDataReceived.Invoke(feeling);
            }

            yield return null;
        }


    }


    //############### UnRegister Events  #####################
    void OnDestroy() {
        BluetoothAdapter.OnDeviceOFF -= HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound -= HandleOnDeviceNotFound;
    }

}
