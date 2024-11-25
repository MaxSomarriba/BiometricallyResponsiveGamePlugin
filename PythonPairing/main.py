from bleak import BleakClient
import asyncio

# if sys.platform == "win32" and (3, 8, 0) <= sys.version_info < (3, 9, 0):
#     UnityEngine.Debug.Log("Setting asyncio WindowsSelectorEventLoopPolicy")
device_address = "E6:7F:A6:74:4F:F0"

# Heart Rate Measurement UUID
HRM_UUID = "00002a37-0000-1000-8000-00805f9b34fb"
x= 0

# Function to parse heart rate data from characteristic value
def parse_heart_rate(data):
    # The first byte contains flags (format, sensor contact, etc.)
    flags = data[0]
    heart_rate_format = flags & 0x01
    
    if heart_rate_format == 0:
        # Heart rate is in 8-bit format
        heart_rate = data[1]
    else:
        # Heart rate is in 16-bit format
        heart_rate = int.from_bytes(data[1:3], byteorder='little')
    
    return heart_rate

# Function to handle notifications from the heart rate monitor
def notification_handler(sender, data):
    heart_rate = parse_heart_rate(data)
    print(f"Heart rate: {heart_rate} bpm")
    if(heart_rate > 80):
        print("Heart rate is too high")
        raise SystemExit("Heart rate exceeded threshold. Stopping program.")

    


async def run():
    # Connect to the heart rate monitor
    async with BleakClient(device_address) as client:
        print(f"Connected: {client.is_connected}")
        x=client
        # Start receiving notifications from the heart rate measurement characteristic
        await client.start_notify(HRM_UUID, notification_handler)
        
        # Keep the connection alive and listen for notifications
        await asyncio.sleep(30)  # Adjust the time as needed
        
        # Stop notifications after done
        await client.stop_notify(HRM_UUID)

async def main():
    # Run the key listener and Bluetooth client concurrently
    await run()

# Run the async function
loop = asyncio.new_event_loop()
asyncio.set_event_loop(loop)
loop.run_in_executor(None, loop.run_until_complete, main())
