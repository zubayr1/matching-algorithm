import sys
from work import  returnfulldata, returnuserdata

alldata = returnfulldata() #from DSO




def matching(userlist, value, TOTALFLEXREQUESTED):
    accepted_offers = {}

    SIGN= 1
    if TOTALFLEXREQUESTED<0:
        SIGN=-1
        TOTALFLEXREQUESTED = abs(TOTALFLEXREQUESTED)

    COUNT=0
    for user in userlist:
            userdata, username = returnuserdata(user) #from users
            COUNT+=1
            for uservalue in userdata['flexOfferList']:
                if value['RequestID']== uservalue['RequestID']:
                    
                    if (SIGN>0 and uservalue['totalFlexOfferedEU']> 0) or (SIGN<0 and uservalue['totalFlexOfferedEU']<0):

                        if SIGN<0 and uservalue['totalFlexOfferedEU']<0:
                            uservalue['totalFlexOfferedEU'] = abs(uservalue['totalFlexOfferedEU'])

                        if TOTALFLEXREQUESTED-uservalue['totalFlexOfferedEU']>0:
                            TOTALFLEXREQUESTED-=uservalue['totalFlexOfferedEU']
                            accepted_offers[username] = (uservalue['totalFlexOfferedEU'])
                        else:
                            accepted_offers[username] = (TOTALFLEXREQUESTED)
                            return accepted_offers
                    
                    print('\n')
    
    if COUNT==len(userlist) and TOTALFLEXREQUESTED>0:
        return accepted_offers



def checkFulmentFactor(accepted_offers, value,TOTALFLEXREQUESTED):
    if sum(accepted_offers.values())/abs(TOTALFLEXREQUESTED) * 100 >= value:
        return True
    return False




def fcfs():
    accepted_offers = {}
    final_accepted_offers = {}

    
    print('First Come First Serve')
    
    for i in alldata:
        if i['marketType']=='fixedPrice':
            for value in i['flexRequestList']:
                if (value['ifFlexRequested'])==False:
                    continue
                print('\n\n\n')
                print('Request for '+value['RequestID']+" "+ str(value['totalFlexRequestedEU']))

                TOTALFLEXREQUESTED = value['totalFlexRequestedEU']

                print('\n')

                userlist = value['loc'].keys()

                accepted_offers = matching(userlist, value, TOTALFLEXREQUESTED)

                if checkFulmentFactor(accepted_offers, value['fullfillmenttFactor'],TOTALFLEXREQUESTED):                
                    final_accepted_offers[value['RequestID']] = accepted_offers
                else:
                    final_accepted_offers[value['RequestID']] = 'fullment factor did not match'


    print(final_accepted_offers)
    return final_accepted_offers



if __name__ == '__main__':
    fcfs()



#   py .\FCFS.py 