import socket
import json
import datetime
import time

commands = {
    0: "Builtin.AddMultiMediaVisualTemplate",
    1: "Builtin.ShadowLanternLTP",
    2: "Builtin.WriteJson",
    3: "Builtin.DownloadFile"
}

def parse_csharp_datetime(time):
    return datetime.datetime.strptime(time, "%Y%m%d %H%M%S").strftime("%Y/%m/%d %H:%M:%S")

if __name__ == '__main__':
    flag = True
    while(flag):
        print("指令列表")
        for c in commands:
            print(c, ") ", commands[c])
        s = int(input("选择一个指令类型>>>").strip())

        print("输入时间 (格式YYYYMMDD HHMMSS)")
        start_time_ = input("开始时间(如果有的话，否则输入.)>>>")
        end_time_ = input("结束时间(如果有的话，否则输入.)>>>")

        st = "2000/01/01 00:00:00"
        et = "2000/01/01 00:00:00"

        if start_time_ is not ".":
            st = parse_csharp_datetime(start_time_)
        if end_time_ is not ".":
            et = parse_csharp_datetime(end_time_)
            
        arg = input("参数>>>")

        command = {
            "command_name": commands[s],
            "start_time": st,
            "end_time": et,
            "args": arg
        }

        json_str = json.dumps(command)
        print(json_str)

        udp_sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        udp_sock.sendto(json_str.strip().encode("utf-8"), ("127.0.0.1", 19260))
        
        time.sleep(1)
        udp_sock.close()

        print("发送至目标设备的 19260 端口，并立即侦听本机发送端口。")
        