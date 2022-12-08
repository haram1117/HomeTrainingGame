using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class WifiSessionController 
{
    public String myIp = "172.30.1.42";
    public int port = 3000;
    private static WifiSessionController wifiSessionController;
    
    #region private members 	
    /// <summary> 	
    /// TCPListener to listen for incomming TCP connection 	
    /// requests. 	
    /// </summary> 	
    private TcpListener tcpListener; 
    /// <summary> 
    /// Background thread for TcpServer workload. 	
    /// </summary> 	
    private Thread tcpListenerThread;  	
    /// <summary> 	
    /// Create handle to connected tcp client. 	
    /// </summary> 	
    private TcpClient connectedTcpClient;
    
    /// <summary> 	
    /// Hadling if ther is event or not 	
    /// </summary> 	
    private bool hasEvent;
    #endregion 	
    

    private WifiSessionController()
    {
        wifiSessionController = this;
        hasEvent = false;
        tcpListenerThread = new Thread(Listen);
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    public static WifiSessionController getInstance()
    {
        if (null == wifiSessionController)
        {
            wifiSessionController = new WifiSessionController();
        }

        return wifiSessionController;
    }
    public bool isEventExist()
    {
        bool result = hasEvent;
        hasEvent = false;
        return result;
    }
    
    private void Listen()
    {
        try { 			
            tcpListener = new TcpListener(IPAddress.Parse(myIp), port); 			
            tcpListener.Start();              
            Debug.Log("Server is listening");              
            Byte[] bytes = new Byte[1024];  			
            while (true) { 				
                using (connectedTcpClient = tcpListener.AcceptTcpClient()) { 					
                    // Get a stream object for reading 					
                    using (NetworkStream stream = connectedTcpClient.GetStream()) { 						
                        int length; 						
                        // Read incomming stream into byte arrary. 						
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 							
                            var incommingData = new byte[length]; 							
                            Array.Copy(bytes, 0, incommingData, 0, length);  							
                            // Convert byte array to string message. 							
                            string clientMessage = Encoding.ASCII.GetString(incommingData); 							
                            Debug.Log("client message received as: " + clientMessage);
                            hasEvent = true;
                        } 					
                    } 				
                } 			
            } 		
        } 		
        catch (SocketException socketException) { 			
            Debug.Log("SocketException " + socketException.ToString()); 		
        }
    }
}
