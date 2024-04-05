package main

import (
	"bufio"
	"fmt"
	"math/rand"
	"net"
	"strings"
	"time"
)

func main() {
	// Start listening on port 23 (default telnet port)
	ln, err := net.Listen("tcp", ":23")
	if err != nil {
		fmt.Println("Error listening:", err.Error())
		return
	}
	defer ln.Close()

	fmt.Println("Telnet server started, listening on port 23")

	for {
		conn, err := ln.Accept()
		if err != nil {
			fmt.Println("Error accepting connection:", err.Error())
			return
		}
		go handleConnection(conn)
	}
}

func handleConnection(conn net.Conn) {
	defer conn.Close()

	// Send initial greeting message
	conn.Write([]byte("Welcome to the Go Telnet server!\r\n"))

	reader := bufio.NewReader(conn)
	for {
		// Read user input
		input, err := reader.ReadString('\n')
		if err != nil {
			fmt.Println("Error reading:", err.Error())
			return
		}
		// Handle the input command
		response := handleCommand(input)
		// Send response back to the client
		conn.Write([]byte(response))

		if strings.TrimSpace(response) == "Goodbye!" {
			return
		}
	}
}

func handleCommand(input string) string {
	// Remove newline character
	input = strings.TrimSuffix(input, "\r\n")
	parts := strings.Split(input, " ")
	command := parts[0]

	fmt.Printf("Received command: %s\n", input)
	// Handle different commands
	switch command {
	case "get_rainfall_intensity":
		if len(parts) == 2 && parts[1] == "zero" {
			return "The rainfall intensity is: 0.0mm/hr\r\n"
		}
		if len(parts) == 2 {
			return "The rainfall intensity is: 1.0mm/hr\r\n"
		}
		return fmt.Sprintf("The rainfall intensity is: %fmm/hr\r\n", rand.Float64())
	case "time":
		return fmt.Sprintf("Current time is: %s\r\n", getTime())
	case "quit":
		return "Goodbye!\r\n"
	default:
		return "Unknown command. Type 'get_rainfall_intensity' for get data, 'time' for current time, or 'quit' to exit.\r\n"
	}
}

func getTime() string {
	return fmt.Sprintf("%s", time.Now().Format("15:04:05"))
}
