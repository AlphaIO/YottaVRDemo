using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TechTweaking.Bluetooth;
using UnityEngine.UI;

[System.Serializable]
public class BluetoothEvent : UnityEngine.Events.UnityEvent<string> { }

public class BluetoothListener : MonoBehaviour {
    public string macAddress;
	public string deviceName = "HMSoft";
    public BluetoothEvent onDataReceived;
    private BluetoothDevice device;
    public FaceController[] FaceControllerArray;
    public Text debugText;

	private char[] charsToTrim = { '{', '}' };
	private string parsedFeeling;

	public Text debugLabel;

	private string[] inputDataVariants = new string[] 
	{
		"{smile, 0,0,0,0,0,0,0,0,0,0}",
		"{anger, 0,0,0,0,0,0,0,0,0,0}",
		"{contempt, 0,0,0,0,0,0,0,0,0,0}",
		"{disgust, 0,0,0,0,0,0,0,0,0,0}",
		"{surprise, 0,0,0,0,0,0,0,0,0,0}",
		"{fear, 0,0,0,0,0,0,0,0,0,0}",
		"{sadness, 0,0,0,0,0,0,0,0,0,0}",
		"{neutral, 0,0,0,0,0,0,0,0,0,0}",

		"{big_wide_smile, 0,0,0,0,0,0,0,0,0,0}",
		"{closed_smile, 0,0,0,0,0,0,0,0,0,0}",
		"{eyebrows_only, 0,0,0,0,0,0,0,0,0,0}",
		"{left_teeth, 0,0,0,0,0,0,0,0,0,0}",
		"{mouth_only, 0,0,0,0,0,0,0,0,0,0}",
		"{open_wide, 0,0,0,0,0,0,0,0,0,0}",
		"{pursed_lips, 0,0,0,0,0,0,0,0,0,0}",
		"{right_teeth, 0,0,0,0,0,0,0,0,0,0}",
		"{smile_and_eyebrow, 0,0,0,0,0,0,0,0,0,0}",
		"{teethview, 0,0,0,0,0,0,0,0,0,0}"
	};

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
		 * 10 equals the char '\n' which is a "new Line" in ASCCI representation, 
		 * so the read() method will retun a packet that was ended by the byte 10. simply read() will read lines.
		 */
        device.setEndByte(10);
    }

    void Start() {
        BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound += HandleOnDeviceNotFound;

		//Android - No bluetooth connection testing
		#if !UNITY_EDITOR
			connect();
			//StartCoroutine (RandomInputData());
		#endif
    }

    void HandleOnDeviceOff(BluetoothDevice dev) {
		if (!string.IsNullOrEmpty (dev.Name)) {
			Debug.Log ("Can't connect to " + dev.Name + ", device is OFF");

			debugLabel.text = "Can't connect to " + dev.Name + ", device is OFF";
			debugText.text = "Can't connect to " + dev.Name + ", device is OFF";
		}

        else if (!string.IsNullOrEmpty(dev.MacAddress)) {
            Debug.Log("Can't connect to " + dev.MacAddress + ", device is OFF");

			debugLabel.text = "Can't connect to " + dev.MacAddress + ", device is OFF";
			debugText.text = "Can't connect to " + dev.MacAddress + ", device is OFF";
        }
    }
    void HandleOnDeviceNotFound(BluetoothDevice dev) {
		if (!string.IsNullOrEmpty (dev.Name)) {
			Debug.Log ("Can't find " + dev.Name + ", device might be OFF or not paird yet ");

			debugLabel.text = "Can't find " + dev.Name + ", device might be OFF or not paird yet ";
			debugText.text = "Can't find " + dev.Name + ", device might be OFF or not paird yet ";
		}
        else if (!string.IsNullOrEmpty(dev.MacAddress)) {
            Debug.Log("Can't find " + dev.MacAddress + ", device is OFF or not paired yet");

			debugLabel.text = "Can't find " + dev.MacAddress + ", device is OFF or not paired yet";
			debugText.text = "Can't find " + dev.MacAddress + ", device is OFF or not paired yet";
        }
    }

    //############### UI BUTTONS #####################
    public void connect()//Connect to the public global variable "device" if it's not null.
    {
		debugLabel.text = "starting to connect";
		debugText.text = "starting to connect";

        if (!BluetoothAdapter.isBluetoothEnabled()) {
            BluetoothAdapter.askEnableBluetooth();

			debugLabel.text = "no bluetooth enabled";
			debugText.text = "no bluetooth enabled";
            return;
        }

		if (device != null) {
			//please read about the different connect method. 
			//this normal_connect(..) method uses nothing fancy, just normal connection. 
			// the other method, connect(...) method,  will try different ways to connect and will take longer time
			// but it's mainly to support a wide variety of devices, and it has bigger chance of connection.
			debugLabel.text = "connecting to device";
			debugText.text = "connecting to device";
			device.connect ();
			StartCoroutine (ManageConnection (device));

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

		debugLabel.text = "check if connected and reading";
		debugText.text = "check if connected and reading";

        while (device.IsConnected && device.IsReading) {

            //polll all available packets
			debugLabel.text = "reading packets";
			debugText.text = "reading packets";

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
                
				//{smile, 0,0,0,0,0,0,0,0,0,0}
				parsedFeeling = System.Text.ASCIIEncoding.ASCII.GetString(packets.Buffer, indx, size).Trim (charsToTrim).Split (',')[0];

				Debug.Log("BLUETOOTH : " + device.Name + " Feels : " + parsedFeeling);
				debugText.text = " Feels : " + parsedFeeling;
				onDataReceived.Invoke(parsedFeeling);

				debugLabel.text = parsedFeeling;
				debugText.text = parsedFeeling;
            }

			yield return new WaitForSecondsRealtime (0.1f);
        }

		debugLabel.text = "not connected, no reading";
		debugText.text = "not connected, no reading";
    }

	//Android - No bluetooth connection testing
	IEnumerator RandomInputData()
	{
		while (true) {
			yield return new WaitForSecondsRealtime (5.0f);

			parsedFeeling = inputDataVariants[Random.Range (0, inputDataVariants.Length)];
			parsedFeeling = parsedFeeling.Trim (charsToTrim).Split (',') [0];
			onDataReceived.Invoke (parsedFeeling);
		}
	}

	// Update is called once per frame
	void Update() {
		/*if (NetworkPlayerCtrl.networkPlayerAuthority != null)
                NetworkPlayerCtrl.networkPlayerAuthority.SetExpression("Joy");*/

		//Unity Editor PC testing
		//Debug input from keyboard
		#if UNITY_EDITOR
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				parsedFeeling = inputDataVariants[0].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
				parsedFeeling = inputDataVariants[1].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
				parsedFeeling = inputDataVariants[2].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
				parsedFeeling = inputDataVariants[3].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
				parsedFeeling = inputDataVariants[4].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha6)) {
				parsedFeeling = inputDataVariants[5].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha7)) {
				parsedFeeling = inputDataVariants[6].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha8)) {
				parsedFeeling = inputDataVariants[7].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			}

			else if (Input.GetKeyDown (KeyCode.F1)) {
				parsedFeeling = inputDataVariants[8].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F2)) {
				parsedFeeling = inputDataVariants[9].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F3)) {
				parsedFeeling = inputDataVariants[10].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F4)) {
				parsedFeeling = inputDataVariants[11].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F5)) {
				parsedFeeling = inputDataVariants[12].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F6)) {
				parsedFeeling = inputDataVariants[13].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F7)) {
				parsedFeeling = inputDataVariants[14].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F8)) {
				parsedFeeling = inputDataVariants[15].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F9)) {
				parsedFeeling = inputDataVariants[16].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F10)) {
				parsedFeeling = inputDataVariants[17].Trim (charsToTrim).Split (',') [0];
				onDataReceived.Invoke (parsedFeeling);
			}
		#endif
	}

    //############### UnRegister Events  #####################
    void OnDestroy() {
        BluetoothAdapter.OnDeviceOFF -= HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound -= HandleOnDeviceNotFound;
    }

}
