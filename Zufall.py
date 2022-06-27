import sys
import random
from work import  returnfulldata, returnuserdata

alldata = returnfulldata()


def matching(userlist, value, TOTALFLEXREQUESTED):
    accepted_offers = {}
    POTENTIALOFFER=[]
    SIGN= 1
    if TOTALFLEXREQUESTED<0:
        SIGN=-1
        TOTALFLEXREQUESTED = abs(TOTALFLEXREQUESTED)

    for user in userlist:
        userdata, username = returnuserdata(user)

        for uservalue in userdata['flexOfferList']:
            if value['RequestID']== uservalue['RequestID']:
                if (SIGN>0 and uservalue['totalFlexOfferedEU']> 0) or (SIGN<0 and uservalue['totalFlexOfferedEU']<0):
                    
                    if SIGN<0 and uservalue['totalFlexOfferedEU']<0:
                        uservalue['totalFlexOfferedEU'] = abs(uservalue['totalFlexOfferedEU'])
                    
                    usertotalflexrequested = uservalue['totalFlexOfferedEU']

                    while usertotalflexrequested!=0:
                        POTENTIALOFFER.append(username)
                        usertotalflexrequested-=1

    random.shuffle(POTENTIALOFFER)
    if len(POTENTIALOFFER)> TOTALFLEXREQUESTED:
        while TOTALFLEXREQUESTED!=0:
            if POTENTIALOFFER[0] in accepted_offers:
                accepted_offers[POTENTIALOFFER[0]]=accepted_offers[POTENTIALOFFER[0]]+1
            else:
                accepted_offers[POTENTIALOFFER[0]]=1
            TOTALFLEXREQUESTED-=1
            
            POTENTIALOFFER.pop(0)
    else:
        for offer in POTENTIALOFFER:
            if offer in accepted_offers:
                accepted_offers[offer]=accepted_offers[offer]+1
            else:
                accepted_offers[offer]=1

    return accepted_offers


def checkFulmentFactor(accepted_offers, value,TOTALFLEXREQUESTED):
    if sum(accepted_offers.values())/abs(TOTALFLEXREQUESTED) * 100 >= value:
        return True
    return False


def zufall():
    accepted_offers = {}
    final_accepted_offers = {}
    
    print('Zufall')

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
    zufall()



#   py .\Zufall.py 