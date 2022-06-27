import sys
from work import  returnfulldata, returnuserdata

alldata = returnfulldata()




def matching(userlist, value, TOTALFLEXREQUESTED):
    accepted_offers = {}
    POTENTIALOFFER={}
    BIDS = []

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
                    BIDS.append((username, uservalue['bidPriceCtpEUList']))


    for name, bidlist in BIDS:
        COUNT=0
        for bids in bidlist:
            if bids<= value['maxPriceCtpEU']:
                POTENTIALOFFER[name+' '+str(COUNT)] = bids
                COUNT+=1

    
    POTENTIALOFFER = {k: v for k, v in sorted(POTENTIALOFFER.items(), key=lambda item: item[1])}

    for i in POTENTIALOFFER:
        name = i.split(' ')[0]
        if TOTALFLEXREQUESTED>1:
            TOTALFLEXREQUESTED-= 1
            if name in accepted_offers:
                accepted_offers[name].append(POTENTIALOFFER[i])
            else:
                accepted_offers[name] = [POTENTIALOFFER[i]]

        else:
            if name in accepted_offers:
                accepted_offers[name].append(POTENTIALOFFER[i])
            else:
                accepted_offers[name] = [POTENTIALOFFER[i]]
            return accepted_offers

    return accepted_offers
    

def checkFulmentFactor(accepted_offers, value,TOTALFLEXREQUESTED):
    SUM = 0

    for i in accepted_offers.values():
        SUM+=len(i)

    if SUM/ abs(TOTALFLEXREQUESTED) * 100 >= value:
        return True

    return False



def mip():
    accepted_offers = {}
    final_accepted_offers = {}

    print('Minimaler Preis ')

    for i in alldata:
        if i['marketType']=='auction':
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
    mip()



#   py .\MiP.py


# 1st unit: 7ct, 2nd unit: 8ct, 3rd ubit: 9ct....