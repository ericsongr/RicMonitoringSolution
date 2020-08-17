import { FuseNavigation } from '@fuse/types';

var navigationContainer: FuseNavigation[] = [
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
                id          : 'apartment', 
                title       : 'Apartment',
                translate   : 'NAV.RENTROOM',
                type        : 'collapsable',
                icon        : 'person',
                children    : [
                    {
                        id: 'ROOMS',
                        title: 'Rooms',
                        type: 'item',
                        url: '/apartment/rooms',
                        exactMatch: true
                    },
                    {
                        id        : 'RENTERS',
                        title     : 'Tenants',
                        type      : 'item',
                        url       :'/apartment/tenants',
                        exactMatch:true
                    },
                    {
                        id        : 'RENTTRANSACTION',
                        title     : 'Tenant Transactions',
                        type      : 'item',
                        url       :'/apartment/tenant-transactions',
                        exactMatch:true
                    }
                ],
            },
            {
                id          : 'administrator', 
                title       : 'Administrator',
                translate   : 'NAV.ADMINISTRATOR',
                type        : 'collapsable',
                icon        : 'person',
                children    : [
                    {
                        id        : 'RENTAUDITS',
                        title     : 'Audits',
                        type      : 'item',
                        url       : '/apartment/audit/logs',
                        exactMatch: true
                    },
                    // {
                    //     id: 'USERS',
                    //     title: 'Users',
                    //     type: 'item',
                    //     url: '/administrator/users',
                    //     exactMatch: true
                    // }
                ],
            }
        ]
    }
];

export const navigation: FuseNavigation[] = navigationContainer;