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
        ]
    }
];
