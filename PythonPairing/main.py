import sys
from bleak import BleakClient
import asyncio

device_address = sys.argv[1]  # First argument is device address
threshold = int(sys.argv[2])  # Second argument is the heart rate threshold
look_for_spikes = bool(sys.argv[3]) # Third argument is if to look for spike threshold
spike_threshold = int(sys.argv[4]) # Fourth argument is the spike threshold
HRM_UUID = "00002a37-0000-1000-8000-00805f9b34fb"
previous_heart_rate = None     # Store the previous heart rate value

# Function to parse heart rate data from characteristic value
def parse_heart_rate(data):
    flags = data[0]
    heart_rate_format = flags & 0x01

    if heart_rate_format == 0:
        heart_rate = data[1]
    else:
        heart_rate = int.from_bytes(data[1:3], byteorder='little')
    
    return heart_rate

# Function to handle notifications from the heart rate monitor
def notification_handler(sender, data):
    global previous_heart_rate
    heart_rate = parse_heart_rate(data)
    print(f"Heart rate: {heart_rate} bpm")

    if previous_heart_rate is not None:
        # Calculate the spike (difference between consecutive readings)
        spike = abs(heart_rate - previous_heart_rate)
        if look_for_spikes:
            if spike > spike_threshold:
                print(f"Spike detected! Heart rate changed by {spike} bpm.")
                raise SystemExit("Spike detected. Stopping program.")
    # Check if the heart rate exceeds the threshold
    if heart_rate > threshold:
        print("Heart rate is too high")
        raise SystemExit("Heart rate exceeded threshold. Stopping program.")
    
    # Update the previous heart rate
    previous_heart_rate = heart_rate

async def run():
    async with BleakClient(device_address) as client:
        print(f"Connected: {client.is_connected}")
        
        # Start receiving notifications from the heart rate measurement characteristic
        await client.start_notify(HRM_UUID, notification_handler)
        
        # Keep the connection alive and listen for notifications
        await asyncio.sleep(15)  # Adjust as needed
        
        # Stop notifications after done
        await client.stop_notify(HRM_UUID)

async def main():
    await run()

# Run the async function
loop = asyncio.new_event_loop()
asyncio.set_event_loop(loop)
loop.run_in_executor(None, loop.run_until_complete, main())
