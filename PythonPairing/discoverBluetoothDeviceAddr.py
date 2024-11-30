import asyncio
from bleak import BleakScanner

async def scan_bluetooth_devices():
    print("Scanning for Bluetooth devices...")

    devices = await BleakScanner.discover()  # Discover nearby devices

    for device in devices:
        print(f"Device: {device.name}, Address: {device.address}, RSSI: {device.rssi}")

# Run the scanning function
asyncio.run(scan_bluetooth_devices())


# optional version that checks if the device is a heart rate monitor
# import asyncio
# from bleak import BleakScanner, BleakClient

# HEART_RATE_SERVICE_UUID = "0000180d-0000-1000-8000-00805f9b34fb"

# async def find_heart_rate_monitors():
#     print("Scanning for Bluetooth devices...")
#     devices = await BleakScanner.discover()
    
#     heart_rate_monitors = []
    
#     for device in devices:
#         try:
#             async with BleakClient(device.address) as client:
#                 services = await client.get_services()
#                 if any(service.uuid == HEART_RATE_SERVICE_UUID for service in services):
#                     print(f"Heart Rate Monitor found: {device.name} ({device.address})")
#                     heart_rate_monitors.append((device.name, device.address))
#         except Exception as e:
#             print(f"Could not connect to {device.name} ({device.address}): {e}")

#     if heart_rate_monitors:
#         print("\nList of Heart Rate Monitors:")
#         for name, address in heart_rate_monitors:
#             print(f"- {name} ({address})")
#     else:
#         print("No Heart Rate Monitors found.")

# # Run the function
# asyncio.run(find_heart_rate_monitors())