using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TechTweaking.Bluetooth;
using UnityEngine.UI;

[System.Serializable]
public class BluetoothEvent : UnityEngine.Events.UnityEvent<string[]> { }

public class BluetoothListener : MonoBehaviour {
	public FaceController[] FaceControllerArray;

	public string macAddress;
	public string deviceName = "HMSoft";

	public BluetoothEvent onDataReceived;
    private BluetoothDevice device;
    
	public Text debugText;
	public Text debugLabel;

	public const float DATA_UPDATE_FREQUENCY = 1.0f / 30.0f;

	private char[] charsToTrim = { '{', '}', '(', ')' };
	private string[] inputDataVariants = new string[] 
	{
		"{neutral, 10,0,0,0,0,0,0,0,0,0}",
		"{smile, 9,0,0,0,0,0,0,0,0,0}",
		"{surprise, 8,0,0,0,0,0,0,0,0,0}",
		"{contempt, 7,0,0,0,0,0,0,0,0,0}",
		"{anger, 5,0,0,0,0,0,0,0,0,0}",
		"{disgust, 3,0,0,0,0,0,0,0,0,0}",
		"{sadness, 1,0,0,0,0,0,0,0,0,0}"//,

		/*"{fear, 0,0,0,0,0,0,0,0,0,0}",
		"{big_wide_smile, 0,0,0,0,0,0,0,0,0,0}",
		"{closed_smile, 0,0,0,0,0,0,0,0,0,0}",
		"{eyebrows_only, 0,0,0,0,0,0,0,0,0,0}",
		"{left_teeth, 0,0,0,0,0,0,0,0,0,0}",
		"{mouth_only, 0,0,0,0,0,0,0,0,0,0}",
		"{open_wide, 0,0,0,0,0,0,0,0,0,0}",
		"{pursed_lips, 0,0,0,0,0,0,0,0,0,0}",
		"{right_teeth, 0,0,0,0,0,0,0,0,0,0}",
		"{smile_and_eyebrow, 0,0,0,0,0,0,0,0,0,0}",
		"{teethview, 0,0,0,0,0,0,0,0,0,0}"*/
	};

	private string[] parsedFeeling;

    void Awake() {
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
		device.ReadingCoroutine = ManageConnection;

		BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;
		BluetoothAdapter.OnDeviceNotFound += HandleOnDeviceNotFound;
		BluetoothAdapter.OnBluetoothON += HandleOnBluetoothON;
		BluetoothAdapter.listenToBluetoothState ();

		if (BluetoothAdapter.isBluetoothEnabled ()) {
			AfterBtEnabled ();
		} else {
			SetDebugText ("Try enable BlueTooth");
			//Ask user to enable BluetoothAdapter.askEnableBluetooth() - app hangs when using this in VR
			//So we using BluetoothAdapter.enableBluetooth() - this force enabling Bluetooth without asking the user
			BluetoothAdapter.enableBluetooth(); //Force Enabling Bluetooth
		}
    }

	void HandleOnBluetoothON ()
	{
		SetDebugText ("HandleOnBluetoothON");

		if (!device.IsConnected && !device.IsReading) {
			BluetoothAdapter.stopListenToBluetoothState ();
			AfterBtEnabled ();
		}
	}

	void Start ()
	{
		#if UNITY_EDITOR
		StartCoroutine (RandomInputData ());
		#endif
	}

	private void AfterBtEnabled ()
	{
		#if !UNITY_EDITOR
		//connect ();

		//Device without BT Yotta testing

		StartCoroutine (RandomInputData ());
		#endif
	}

	private void SetDebugText (string txt)
	{
		debugLabel.text = txt;
		debugText.text = txt;
	}

    void HandleOnDeviceOff(BluetoothDevice dev) {
		if (!string.IsNullOrEmpty (dev.Name)) {
			Debug.Log ("Can't connect to " + dev.Name + ", device is OFF");
			SetDebugText ("Can't connect to " + dev.Name + ", device is OFF");
		}

        else if (!string.IsNullOrEmpty(dev.MacAddress)) {
            Debug.Log("Can't connect to " + dev.MacAddress + ", device is OFF");
			SetDebugText ("Can't connect to " + dev.MacAddress + ", device is OFF");
		}
    }
    void HandleOnDeviceNotFound(BluetoothDevice dev) {
		if (!string.IsNullOrEmpty (dev.Name)) {
			Debug.Log ("Can't find " + dev.Name + ", device might be OFF or not paird yet ");
			SetDebugText ("Can't find " + dev.Name + ", device might be OFF or not paird yet ");
		}
        else if (!string.IsNullOrEmpty(dev.MacAddress)) {
            Debug.Log("Can't find " + dev.MacAddress + ", device is OFF or not paired yet");
			SetDebugText ("Can't find " + dev.MacAddress + ", device is OFF or not paired yet");
		}
    }

    //############### Starting connection #####################
    public void connect()//Connect to the public global variable "device" if it's not null.
    {
		SetDebugText ("Starting to connect");

		if (device != null) {
			//please read about the different connect method. 
			//this normal_connect(..) method uses nothing fancy, just normal connection. 
			// the other method, connect(...) method,  will try different ways to connect and will take longer time
			// but it's mainly to support a wide variety of devices, and it has bigger chance of connection.
			SetDebugText ("Connecting to device");
			device.connect ();
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

		SetDebugText ("Connected & Can read");

        while (device.IsReading) 
		{
			//SetDebugText ("device.IsReading");

			/*if (device.IsDataAvailable) {
				//{smile, 0,0,0,0,0,0,0,0,0,0}
				//SetDebugText ("device.IsDataAvailable");
				byte [] msg = device.read ();//because we called setEndByte(10)..read will always return a packet excluding the last byte 10.

				if (msg != null && msg.Length > 0) {
					//SetDebugText (System.Text.ASCIIEncoding.ASCII.GetString (msg));
					parsedFeeling = System.Text.ASCIIEncoding.ASCII.GetString (msg).Trim (charsToTrim).Split (',')[0];

					Debug.Log("BLUETOOTH : " + device.Name + " Feels : " + parsedFeeling);
					SetDebugText (parsedFeeling);

					onDataReceived.Invoke(parsedFeeling);
				}
            }

			yield return new WaitForSecondsRealtime (DATA_UPDATE_FREQUENCY);*/

			//----------------High Rate Reading--------------
			if (device.IsDataAvailable) {
				//polll all available packets
				BtPackets packets = device.readAllPackets ();

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

					int indx = packets.get_packet_offset_index (packets.Count - 1);
					int size = packets.get_packet_size (packets.Count - 1);

					parsedFeeling = System.Text.ASCIIEncoding.ASCII.GetString (packets.Buffer, indx, size).Trim (charsToTrim).Split (',');
					Debug.Log("BLUETOOTH : " + device.Name + " Feels : " + parsedFeeling[0]);
					SetDebugText (parsedFeeling[0]);
					onDataReceived.Invoke(parsedFeeling);
				}

				yield return new WaitForSecondsRealtime (DATA_UPDATE_FREQUENCY);;
			}
        }

		SetDebugText ("Done Reading");
    }

	//############### UnRegister Events  #####################
	void OnDestroy() {
		BluetoothAdapter.OnDeviceOFF -= HandleOnDeviceOff;
		BluetoothAdapter.OnDeviceNotFound -= HandleOnDeviceNotFound;
		BluetoothAdapter.OnBluetoothON -= HandleOnBluetoothON;
	}

	//################### Editor Debug ###########################
	//Android - No bluetooth connection testing
	IEnumerator RandomInputData()
	{
		while (true) {
			yield return new WaitForSecondsRealtime (1.5f);

			parsedFeeling = inputDataVariants[Random.Range (0, inputDataVariants.Length)].Trim (charsToTrim).Split (',');
			SetDebugText (parsedFeeling[0]);
			onDataReceived.Invoke (parsedFeeling);
		}
	}

	/*public void ReadRandomEmotion ()
	{
		parsedFeeling = inputDataVariants[Random.Range (0, inputDataVariants.Length)].Trim (charsToTrim).Split (',') [0];
		onDataReceived.Invoke (parsedFeeling);
		SetDebugText (parsedFeeling);
	}

	public void SetNeutralEmotion ()
	{
		parsedFeeling = "neutral";
		onDataReceived.Invoke (parsedFeeling);
		SetDebugText (parsedFeeling);
	}*/

	// Update is called once per frame
	void Update() {
		/*if (NetworkPlayerCtrl.networkPlayerAuthority != null)
                NetworkPlayerCtrl.networkPlayerAuthority.SetExpression("Joy");*/

		//Unity Editor PC testing
		//Debug input from keyboard
		#if UNITY_EDITOR
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				parsedFeeling = inputDataVariants[0].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
				parsedFeeling = inputDataVariants[1].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
				parsedFeeling = inputDataVariants[2].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
				parsedFeeling = inputDataVariants[3].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
				parsedFeeling = inputDataVariants[4].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha6)) {
				parsedFeeling = inputDataVariants[5].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha7)) {
				parsedFeeling = inputDataVariants[6].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			} else if (Input.GetKeyDown (KeyCode.Alpha8)) {
				parsedFeeling = inputDataVariants[7].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			}

			else if (Input.GetKeyDown (KeyCode.F1)) {
				parsedFeeling = inputDataVariants[8].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F2)) {
				parsedFeeling = inputDataVariants[9].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F3)) {
				parsedFeeling = inputDataVariants[10].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F4)) {
				parsedFeeling = inputDataVariants[11].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F5)) {
				parsedFeeling = inputDataVariants[12].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F6)) {
				parsedFeeling = inputDataVariants[13].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F7)) {
				parsedFeeling = inputDataVariants[14].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F8)) {
				parsedFeeling = inputDataVariants[15].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F9)) {
				parsedFeeling = inputDataVariants[16].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			}
			else if (Input.GetKeyDown (KeyCode.F10)) {
				parsedFeeling = inputDataVariants[17].Trim (charsToTrim).Split (',');
				onDataReceived.Invoke (parsedFeeling);
			}
		#endif
	}
}
