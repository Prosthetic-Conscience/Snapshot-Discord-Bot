# Snapshot-Discord-Bot
A bot to iterate through a discord server and upload files in xml format with all data requested
Command:Roles
        Channels
        Users
        RemoveDupes
        
Commands give an XML file that can be downloaded with details of each requested dataset with all data nested in schema appropriate fashion

Future plans include the abilty to visualize said data (can be done with Visio currently) in an informative manner. The abilty to load said xml files is planned but not implemented, as it would need to be done on a server-wide scale, with an XML file that may exceed timeouts to save on larger servers. unless staged at a roles/channel/users/ fashion.

The removeDupes function finds users UUID to find multiple names from the same user within a server
