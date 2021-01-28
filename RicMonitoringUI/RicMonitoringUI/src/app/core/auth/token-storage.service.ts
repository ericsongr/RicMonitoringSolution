import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AccessData } from './access-data';

@Injectable()
export class TokenStorage {

	/**
	 * Get user roles in JSON string
	 * @returns {Observable<any>}
	 */
	public getUserRoles(): Observable<any> {
		const roles: any = sessionStorage.getItem('userRoles');
		try {
			return of(JSON.parse(roles));
		} catch (e) { }
	}

	/**
	 * Set user roles
	 * @param roles
	 * @returns {TokenStorage}
	 */
	public setUserRoles(roles: any): any {
		if (roles != null) {
			sessionStorage.setItem('userRoles', JSON.stringify(roles));
		}
		return this;
	}

	public setAccessData(accessData: any): any {
		if (accessData != null) {
			sessionStorage.removeItem('accessData');
			sessionStorage.setItem('accessData', JSON.stringify(accessData));
		}
		return this;
	}

	public getAccessData(): Observable<any> {
		const accessData: any = sessionStorage.getItem('accessData');
		try {
			return of(JSON.parse(accessData));
		} catch (e) { }
	}

	public getAccessData2(): AccessData {
		var item = sessionStorage.getItem('accessData');
		return JSON.parse(item);
	}

	public setUserName(userName: string): any {
		sessionStorage.setItem('userName', userName);
		return this;
	}

	public getUserName(): Observable<string> {
		const token: string = <string>sessionStorage.getItem('userName');
		return of(token);
	}

	public setMemberStatus(status: string): void {
		sessionStorage.removeItem('memberStatus');
		sessionStorage.setItem('memberStatus', status);
	}

	public getMemberStatus(): string {
		return sessionStorage.getItem('memberStatus');
	}

	/**
	 * Remove tokens
	 */
	public clear() {
		sessionStorage.removeItem('userRoles');
		sessionStorage.removeItem('accessData');
		sessionStorage.removeItem('userName');
	}
}
