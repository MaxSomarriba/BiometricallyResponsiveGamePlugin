# Bluetooth scanner
# Prints the name and address of every nearby Bluetooth LE device

# E6:7F:A6:74:4F:F0: HW706-0040796

# import asyncio
# from bleak import BleakScanner

# async def main():
#     devices = await BleakScanner.discover()
#     for device in devices:
#         print(device)

# asyncio.run(main())

import asyncio
from bleak import BleakClient

# Replace with your device's Bluetooth address
device_address = "E6:7F:A6:74:4F:F0"  # Replace with your heart rate monitor's address

# Heart Rate Measurement UUID (standard for BLE heart rate monitors)
HRM_UUID = "00002a37-0000-1000-8000-00805f9b34fb"

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

async def run():
    # Connect to the heart rate monitor
    async with BleakClient(device_address) as client:
        print(f"Connected: {client.is_connected}")

        # Start receiving notifications from the heart rate measurement characteristic
        await client.start_notify(HRM_UUID, notification_handler)
        
        # Keep the connection alive and listen for notifications
        await asyncio.sleep(60)  # Adjust the time as needed
        
        # Stop notifications after done
        await client.stop_notify(HRM_UUID)

# Run the async function
loop = asyncio.get_event_loop()
loop.run_until_complete(run())
