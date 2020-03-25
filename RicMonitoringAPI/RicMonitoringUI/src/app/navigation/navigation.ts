import { FuseNavigation } from '@fuse/types';

export const navigation: FuseNavigation[] = [
    {
        id       : 'applications',
        title    : 'Applications',
        translate: 'NAV.APPLICATIONS',
        type     : 'group',
        children : [
            // {
            //     id       : 'sample',
            //     title    : 'Sample',
            //     translate: 'NAV.SAMPLE.TITLE',
            //     type     : 'item',
            //     icon     : 'email',
            //     url      : '/sample',
            //     badge    : {
            //         title    : '25',
            //         translate: 'NAV.SAMPLE.BADGE',
            //         bg       : '#F44336',
            //         fg       : '#FFFFFF'
            //     }
            // },
            {
                id          : 'rentroom', 
                title       : 'Rent Room',
                translate   : 'NAV.RENTROOM',
                type        : 'collapsable',
                icon        : 'person',
                children    : [
                    {
                        id: 'ROOMS',
                        title: 'Rooms',
                        type: 'item',
                        url: '/apps/rent-room/rooms',
                        exactMatch: true
                    },
                    {
                        id        : 'RENTERS',
                        title     : 'Tenants',
                        type      : 'item',
                        url       :'/apps/rent-room/tenants',
                        exactMatch:true
                    },
                    {
                        id        : 'RENTTRANSACTION',
                        title     : 'Tenant Transactions',
                        type      : 'item',
                        url       :'/apps/rent-room/tenant-transactions',
                        exactMatch:true
                    }
                ],
            },
        ]
    }
];
